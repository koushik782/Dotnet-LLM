# DevAssistant Setup Test Script
Write-Host "🧪 Testing DevAssistant Setup..." -ForegroundColor Green
Write-Host ""

# Test Docker services
Write-Host "1. Testing Docker services..." -ForegroundColor Yellow
try {
    $services = docker-compose ps --format json | ConvertFrom-Json
    foreach ($service in $services) {
        $status = if ($service.State -eq "running") { "✅" } else { "❌" }
        Write-Host "   $status $($service.Service): $($service.State)" -ForegroundColor $(if ($service.State -eq "running") { "Green" } else { "Red" })
    }
} catch {
    Write-Host "   ❌ Failed to check Docker services" -ForegroundColor Red
}

Write-Host ""

# Test Backend API
Write-Host "2. Testing Backend API..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:7001/health" -Method Get -TimeoutSec 10
    Write-Host "   ✅ Backend API is responding" -ForegroundColor Green
    Write-Host "   📊 Health Status: $($response.isHealthy)" -ForegroundColor Cyan
} catch {
    Write-Host "   ❌ Backend API is not responding" -ForegroundColor Red
    Write-Host "      Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Ollama connection
Write-Host "3. Testing Ollama connection..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:7001/api/chat/health" -Method Get -TimeoutSec 10
    Write-Host "   ✅ Ollama connection: $($response.ollamaStatus)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Cannot check Ollama status" -ForegroundColor Red
}

# Test Frontend
Write-Host "4. Testing Frontend..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:4200" -Method Get -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "   ✅ Frontend is accessible" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Frontend returned status: $($response.StatusCode)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   ❌ Frontend is not accessible" -ForegroundColor Red
    Write-Host "      Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Database
Write-Host "5. Testing Database..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "http://localhost:7001/health" -Method Get -TimeoutSec 10
    Write-Host "   ✅ Database health check passed" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Database health check failed" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎯 Setup Test Summary:" -ForegroundColor Green
Write-Host "   - If all tests show ✅, your DevAssistant is ready!" -ForegroundColor White
Write-Host "   - If you see ❌, check the service status and logs" -ForegroundColor White
Write-Host "   - Use 'docker-compose logs [service-name]' to debug issues" -ForegroundColor White

Write-Host ""
Write-Host "🚀 Ready to use DevAssistant!" -ForegroundColor Green
Write-Host "   Frontend: http://localhost:4200" -ForegroundColor Cyan
Write-Host "   Backend: http://localhost:7001" -ForegroundColor Cyan
Write-Host "   Swagger: http://localhost:7001" -ForegroundColor Cyan 