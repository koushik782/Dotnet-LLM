using Microsoft.AspNetCore.Mvc;
using DevAssistant.Api.Services;
using System.Text.Json;
using System.Text;

namespace DevAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly OllamaClient _ollamaClient;
    private readonly PromptTemplateService _promptTemplateService;
    private readonly ILogger<ChatController> _logger;
    private readonly DevAssistantDbContext? _dbContext;

    public ChatController(
        OllamaClient ollamaClient,
        PromptTemplateService promptTemplateService,
        ILogger<ChatController> logger,
        IServiceProvider serviceProvider)
    {
        _ollamaClient = ollamaClient;
        _promptTemplateService = promptTemplateService;
        _logger = logger;
        
        // Try to get database context (it might not be available)
        try
        {
            _dbContext = serviceProvider.GetService<DevAssistantDbContext>();
        }
        catch
        {
            _dbContext = null;
        }
    }

    [HttpPost("stream")]
    public async Task StreamChatAsync([FromBody] ChatRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserInput))
        {
            Response.StatusCode = 400;
            await Response.WriteAsync("User input is required", cancellationToken);
            return;
        }

        // Set up SSE headers
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        Response.Headers.Add("Access-Control-Allow-Origin", "*");
        Response.Headers.Add("Access-Control-Allow-Headers", "Cache-Control");

        try
        {
            // Check if Ollama is healthy before processing
            var isHealthy = await _ollamaClient.IsHealthyAsync(cancellationToken);
            if (!isHealthy)
            {
                await SendSseEventAsync("error", "Ollama service is not available. Please ensure Ollama is running.", cancellationToken);
                return;
            }

            // Get the formatted prompt using the selected template
            var prompt = _promptTemplateService.GetPrompt(request.Template ?? "general", request.UserInput);
            
            _logger.LogInformation("Processing chat request with template: {Template}", request.Template ?? "general");

            // Send start event
            await SendSseEventAsync("start", "Starting response...", cancellationToken);

            var responseBuilder = new StringBuilder();
            
            // Stream the response from Ollama
            await foreach (var chunk in _ollamaClient.StreamChatAsync(prompt, request.Model ?? "mistral", cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                responseBuilder.Append(chunk);
                
                // Send each chunk to the client
                await SendSseEventAsync("chunk", chunk, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            // Send completion event
            await SendSseEventAsync("complete", "Response completed", cancellationToken);
            
            _logger.LogInformation("Chat request completed successfully");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Ollama service error");
            await SendSseEventAsync("error", ex.Message, cancellationToken);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Request timeout");
            await SendSseEventAsync("error", "Request timed out. Please try again.", cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Chat request was cancelled");
            await SendSseEventAsync("cancelled", "Request was cancelled", cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during chat processing");
            await SendSseEventAsync("error", "An unexpected error occurred. Please try again.", cancellationToken);
        }
    }

    [HttpGet("templates")]
    public ActionResult<IEnumerable<PromptTemplate>> GetTemplates()
    {
        return Ok(_promptTemplateService.GetAvailableTemplates());
    }

    [HttpGet("health")]
    public async Task<ActionResult<HealthStatus>> GetHealthAsync(CancellationToken cancellationToken)
    {
        var isOllamaHealthy = await _ollamaClient.IsHealthyAsync(cancellationToken);
        var isDatabaseHealthy = _dbContext != null;
        
        return Ok(new HealthStatus
        {
            IsHealthy = isOllamaHealthy && isDatabaseHealthy,
            OllamaStatus = isOllamaHealthy ? "Connected" : "Disconnected",
            DatabaseStatus = isDatabaseHealthy ? "Connected" : "Not Available",
            Timestamp = DateTime.UtcNow
        });
    }

    private async Task SendSseEventAsync(string eventType, string data, CancellationToken cancellationToken)
    {
        var sseEvent = new SseEvent
        {
            Type = eventType,
            Data = data,
            Timestamp = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(sseEvent);
        var formattedData = $"data: {json}\n\n";
        
        await Response.WriteAsync(formattedData, cancellationToken);
    }
}

public class ChatRequest
{
    public string UserInput { get; set; } = string.Empty;
    public string? Template { get; set; }
    public string? Model { get; set; }
}

public class SseEvent
{
    public string Type { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class HealthStatus
{
    public bool IsHealthy { get; set; }
    public string OllamaStatus { get; set; } = string.Empty;
    public string DatabaseStatus { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
} 