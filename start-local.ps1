# DevAssistant Local Development Startup Script
Write-Host "🚀 Starting DevAssistant Local Development..." -ForegroundColor Green
Write-Host ""

# Check if .NET is available
Write-Host "1. Checking .NET installation..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "   ✅ .NET $dotnetVersion is available" -ForegroundColor Green
} catch {
    Write-Host "   ❌ .NET is not installed. Please install .NET 8 SDK first." -ForegroundColor Red
    Write-Host "      Download from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    exit 1
}

# Check if Node.js is available
Write-Host "2. Checking Node.js installation..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    Write-Host "   ✅ Node.js $nodeVersion is available" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Node.js is not installed. Please install Node.js 18+ first." -ForegroundColor Red
    Write-Host "      Download from: https://nodejs.org/" -ForegroundColor Yellow
    exit 1
}

# Check if Ollama is available
Write-Host "3. Checking Ollama installation..." -ForegroundColor Yellow
try {
    $ollamaResponse = Invoke-RestMethod -Uri "http://localhost:11434/api/tags" -Method Get -TimeoutSec 5
    Write-Host "   ✅ Ollama is running" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Ollama is not running. Please start Ollama first:" -ForegroundColor Yellow
    Write-Host "      1. Install Ollama from https://ollama.ai/" -ForegroundColor White
    Write-Host "      2. Run: ollama serve" -ForegroundColor White
    Write-Host "      3. Run: ollama pull mistral" -ForegroundColor White
    Write-Host ""
    Write-Host "   The app will work without Ollama, but AI features won't function." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "4. Starting Backend API..." -ForegroundColor Yellow
Write-Host "   Starting .NET 8 Web API..." -ForegroundColor White

# Start backend in background
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\DevAssistant.Api'; dotnet run"

Write-Host "   ✅ Backend started in new PowerShell window" -ForegroundColor Green
Write-Host "   📍 Backend will be available at: https://localhost:7001" -ForegroundColor Cyan

Write-Host ""
Write-Host "5. Starting Frontend..." -ForegroundColor Yellow
Write-Host "   Installing dependencies and starting Angular..." -ForegroundColor White

# Start frontend in background
Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd '$PWD\DevAssistant.Frontend'; npm install; npm start"

Write-Host "   ✅ Frontend started in new PowerShell window" -ForegroundColor Green
Write-Host "   📍 Frontend will be available at: http://localhost:4200" -ForegroundColor Cyan

Write-Host ""
Write-Host "🎉 DevAssistant is starting up!" -ForegroundColor Green
Write-Host ""
Write-Host "📱 Frontend: http://localhost:4200" -ForegroundColor Cyan
Write-Host "🔧 Backend API: https://localhost:7001" -ForegroundColor Cyan
Write-Host "📚 Swagger UI: https://localhost:7001" -ForegroundColor Cyan
Write-Host ""
Write-Host "⏳ Please wait a few minutes for both services to fully start..." -ForegroundColor Yellow
Write-Host ""
Write-Host "Press any key to open the frontend in your browser..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')

# Open frontend in browser
Start-Process "http://localhost:4200" 