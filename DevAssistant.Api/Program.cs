using Microsoft.EntityFrameworkCore;
using DevAssistant.Api.Data;
using DevAssistant.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework (make it optional)
try
{
    builder.Services.AddDbContext<DevAssistantDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection") ?? 
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DevAssistantDb;Integrated Security=true;MultipleActiveResultSets=true"));
    
    Console.WriteLine("✅ Database context configured successfully.");
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️  Database context configuration failed: {ex.Message}");
    Console.WriteLine("   The app will run without database features.");
}

// Add HttpClient for OllamaClient
builder.Services.AddHttpClient<OllamaClient>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(5); // Allow for long model responses
});

// Register application services
builder.Services.AddScoped<PromptTemplateService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "DevAssistant API", 
        Version = "v1",
        Description = "A developer assistant API powered by Ollama"
    });
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevAssistant API V1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularDev");

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

// Try to initialize database if context is available
try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<DevAssistantDbContext>();
    if (context != null)
    {
        context.Database.EnsureCreated();
        Console.WriteLine("✅ Database initialized successfully.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️  Database initialization failed: {ex.Message}");
    Console.WriteLine("   The app will continue without database features.");
}

Console.WriteLine("🚀 DevAssistant API is starting...");
Console.WriteLine("📝 Swagger UI available at: https://localhost:7001");
Console.WriteLine("🏥 Health check available at: https://localhost:7001/health");
Console.WriteLine("💬 Chat API available at: https://localhost:7001/api/chat");
Console.WriteLine("⚠️  Make sure Ollama is running at http://localhost:11434");
Console.WriteLine("💡 If you don't have Ollama, the app will still run but AI features won't work.");

app.Run(); 