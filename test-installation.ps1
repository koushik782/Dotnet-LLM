# DevAssistant Installation Test Script
Write-Host "🧪 Testing DevAssistant Installation Requirements..." -ForegroundColor Green
Write-Host ""

$allGood = $true

# Test .NET
Write-Host "1. Testing .NET 8 SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    if ($dotnetVersion -like "8.*") {
        Write-Host "   ✅ .NET $dotnetVersion is installed and working" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  .NET $dotnetVersion is installed but not version 8.x" -ForegroundColor Yellow
        Write-Host "      Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
        $allGood = $false
    }
} catch {
    Write-Host "   ❌ .NET is not installed" -ForegroundColor Red
    Write-Host "      Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
    $allGood = $false
}

# Test Node.js
Write-Host "2. Testing Node.js..." -ForegroundColor Yellow
try {
    $nodeVersion = node --version
    $nodeMajor = [int]($nodeVersion -replace 'v', '' -split '\.')[0]
    if ($nodeMajor -ge 18) {
        Write-Host "   ✅ Node.js $nodeVersion is installed and compatible" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Node.js $nodeVersion is installed but version $nodeMajor.x is too old" -ForegroundColor Yellow
        Write-Host "      Please install Node.js 18+ from: https://nodejs.org/" -ForegroundColor White
        $allGood = $false
    }
} catch {
    Write-Host "   ❌ Node.js is not installed" -ForegroundColor Red
    Write-Host "      Please install Node.js 18+ from: https://nodejs.org/" -ForegroundColor White
    $allGood = $false
}

# Test npm
Write-Host "3. Testing npm..." -ForegroundColor Yellow
try {
    $npmVersion = npm --version
    Write-Host "   ✅ npm $npmVersion is available" -ForegroundColor Green
} catch {
    Write-Host "   ❌ npm is not available" -ForegroundColor Red
    Write-Host "      This usually means Node.js wasn't installed properly" -ForegroundColor White
    $allGood = $false
}

# Test Ollama (optional)
Write-Host "4. Testing Ollama (optional)..." -ForegroundColor Yellow
try {
    $ollamaVersion = ollama --version
    Write-Host "   ✅ Ollama $ollamaVersion is installed" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Ollama is not installed (optional for AI features)" -ForegroundColor Yellow
    Write-Host "      Install from: https://ollama.ai/ for AI chat functionality" -ForegroundColor White
}

# Test SQL Server LocalDB (optional)
Write-Host "5. Testing SQL Server LocalDB (optional)..." -ForegroundColor Yellow
try {
    $sqlcmd = sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION" -h -1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ SQL Server LocalDB is available" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  SQL Server LocalDB might not be working properly" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   ⚠️  SQL Server LocalDB is not installed (optional for database features)" -ForegroundColor Yellow
    Write-Host "      Install from: https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb" -ForegroundColor White
}

Write-Host ""
Write-Host "🎯 Installation Test Summary:" -ForegroundColor Green

if ($allGood) {
    Write-Host "   ✅ All required tools are installed and working!" -ForegroundColor Green
    Write-Host "   🚀 You can now run DevAssistant using:" -ForegroundColor White
    Write-Host "      PowerShell: .\start-local.ps1" -ForegroundColor Cyan
    Write-Host "      Command Prompt: start-local.bat" -ForegroundColor Cyan
} else {
    Write-Host "   ❌ Some required tools are missing or incompatible" -ForegroundColor Red
    Write-Host "   📋 Please install the missing tools and run this test again" -ForegroundColor White
    Write-Host "   📖 See INSTALLATION.md for detailed instructions" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🔧 Quick Fix Commands:" -ForegroundColor Green
Write-Host "   - .NET 8 SDK: Download from https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
Write-Host "   - Node.js 18+: Download from https://nodejs.org/" -ForegroundColor White
Write-Host "   - Ollama: Download from https://ollama.ai/" -ForegroundColor White

Write-Host ""
Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown') 