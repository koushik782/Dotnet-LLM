# DevAssistant Development Startup Script
Write-Host "🚀 Starting DevAssistant Development Environment..." -ForegroundColor Green

# Check if Docker is running
Write-Host "Checking Docker status..." -ForegroundColor Yellow
try {
    docker version | Out-Null
    Write-Host "✅ Docker is running" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
    exit 1
}

# Start the services
Write-Host "Starting services with Docker Compose..." -ForegroundColor Yellow
docker-compose up -d

# Wait for services to be ready
Write-Host "Waiting for services to be ready..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check service status
Write-Host "Checking service status..." -ForegroundColor Yellow
docker-compose ps

Write-Host ""
Write-Host "🎉 DevAssistant is starting up!" -ForegroundColor Green
Write-Host ""
Write-Host "📱 Frontend: http://localhost:4200" -ForegroundColor Cyan
Write-Host "🔧 Backend API: http://localhost:7001" -ForegroundColor Cyan
Write-Host "📚 Swagger UI: http://localhost:7001" -ForegroundColor Cyan
Write-Host "🗄️  SQL Server: localhost,1433" -ForegroundColor Cyan
Write-Host "🤖 Ollama: http://localhost:11434" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  Don't forget to pull Ollama models:" -ForegroundColor Yellow
Write-Host "   docker exec -it dotnet_llm-ollama-1 ollama pull mistral" -ForegroundColor White
Write-Host "   docker exec -it dotnet_llm-ollama-1 ollama pull codellama" -ForegroundColor White
Write-Host ""
Write-Host "Press any key to open the frontend in your browser..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

# Open frontend in browser
Start-Process "http://localhost:4200" 