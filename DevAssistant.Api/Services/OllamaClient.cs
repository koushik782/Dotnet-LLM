using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevAssistant.Api.Services;

public sealed class OllamaClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly ILogger<OllamaClient> _logger;
    private readonly string _baseUrl;

    public OllamaClient(HttpClient httpClient, ILogger<OllamaClient> logger, IConfiguration configuration)
    {
        _http = httpClient;
        _logger = logger;
        _baseUrl = configuration.GetValue<string>("Ollama:BaseUrl") ?? "http://localhost:11434";
    }

    public async IAsyncEnumerable<string> StreamChatAsync(
        string prompt, 
        string model = "mistral", 
        CancellationToken cancellationToken = default)
    {
        var requestPayload = new OllamaRequest
        {
            Model = model,
            Prompt = prompt,
            Stream = true,
            Options = new OllamaOptions
            {
                Temperature = 0.7f,
                TopP = 0.9f,
                TopK = 40
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/api/generate")
        {
            Content = JsonContent.Create(requestPayload)
        };

        try
        {
            _logger.LogInformation("Sending request to Ollama with model: {Model}", model);
            
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("Ollama request failed with status {StatusCode}: {Error}", 
                    response.StatusCode, errorContent);
                throw new HttpRequestException($"Ollama request failed: {response.StatusCode} - {errorContent}");
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);
            
            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                OllamaResponse? ollamaResponse = null;
                try
                {
                    ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(line);
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning("Failed to parse Ollama response line: {Line}. Error: {Error}", line, ex.Message);
                    continue;
                }

                if (ollamaResponse?.Response != null)
                {
                    yield return ollamaResponse.Response;
                }

                if (ollamaResponse?.Done == true)
                {
                    _logger.LogInformation("Ollama streaming completed");
                    break;
                }
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to Ollama. Is Ollama running at {BaseUrl}?", _baseUrl);
            throw new InvalidOperationException("Unable to connect to Ollama. Please ensure Ollama is running and accessible.", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogError(ex, "Ollama request timed out");
            throw new TimeoutException("Ollama request timed out. The model might be loading or overloaded.", ex);
        }
    }

    public async Task<bool> IsHealthyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var response = await _http.GetAsync($"{_baseUrl}/api/tags", cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _http?.Dispose();
    }
}

public class OllamaRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("stream")]
    public bool Stream { get; set; } = true;

    [JsonPropertyName("options")]
    public OllamaOptions? Options { get; set; }
}

public class OllamaOptions
{
    [JsonPropertyName("temperature")]
    public float Temperature { get; set; } = 0.7f;

    [JsonPropertyName("top_p")]
    public float TopP { get; set; } = 0.9f;

    [JsonPropertyName("top_k")]
    public int TopK { get; set; } = 40;
}

public class OllamaResponse
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string? Response { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("context")]
    public int[]? Context { get; set; }

    [JsonPropertyName("total_duration")]
    public long? TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public long? LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int? PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public long? PromptEvalDuration { get; set; }

    [JsonPropertyName("eval_count")]
    public int? EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public long? EvalDuration { get; set; }
} 