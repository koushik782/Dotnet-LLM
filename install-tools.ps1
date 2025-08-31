# DevAssistant Tools Auto-Installer
Write-Host "🚀 DevAssistant Tools Auto-Installer" -ForegroundColor Green
Write-Host "This script will install all required tools automatically" -ForegroundColor White
Write-Host ""

# Check if running as Administrator
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")
if (-not $isAdmin) {
    Write-Host "❌ This script requires Administrator privileges" -ForegroundColor Red
    Write-Host "   Please right-click PowerShell and select 'Run as Administrator'" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Press any key to exit..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
    exit 1
}

# Check if winget is available
Write-Host "1. Checking winget availability..." -ForegroundColor Yellow
try {
    $wingetVersion = winget --version
    Write-Host "   ✅ winget $wingetVersion is available" -ForegroundColor Green
} catch {
    Write-Host "   ❌ winget is not available" -ForegroundColor Red
    Write-Host "      Please install winget or use manual installation from INSTALLATION.md" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Press any key to exit..."
    $null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
    exit 1
}

Write-Host ""
Write-Host "2. Installing .NET 8 SDK..." -ForegroundColor Yellow
try {
    Write-Host "   Installing Microsoft.DotNet.SDK.8..." -ForegroundColor White
    winget install Microsoft.DotNet.SDK.8 --accept-source-agreements --accept-package-agreements
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ .NET 8 SDK installed successfully" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Failed to install .NET 8 SDK" -ForegroundColor Red
    }
} catch {
    Write-Host "   ❌ Error installing .NET 8 SDK: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "3. Installing Node.js..." -ForegroundColor Yellow
try {
    Write-Host "   Installing OpenJS.NodeJS..." -ForegroundColor White
    winget install OpenJS.NodeJS --accept-source-agreements --accept-package-agreements
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Node.js installed successfully" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Failed to install Node.js" -ForegroundColor Red
    }
} catch {
    Write-Host "   ❌ Error installing Node.js: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "4. Installing Ollama..." -ForegroundColor Yellow
try {
    Write-Host "   Installing Ollama.Ollama..." -ForegroundColor White
    winget install Ollama.Ollama --accept-source-agreements --accept-package-agreements
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Ollama installed successfully" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Failed to install Ollama" -ForegroundColor Red
    }
} catch {
    Write-Host "   ❌ Error installing Ollama: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "5. Installing SQL Server LocalDB..." -ForegroundColor Yellow
try {
    Write-Host "   Installing Microsoft.SQLServer.LocalDB..." -ForegroundColor White
    winget install Microsoft.SQLServer.LocalDB --accept-source-agreements --accept-package-agreements
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ SQL Server LocalDB installed successfully" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Failed to install SQL Server LocalDB" -ForegroundColor Red
    }
} catch {
    Write-Host "   ❌ Error installing SQL Server LocalDB: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "🎉 Installation Complete!" -ForegroundColor Green
Write-Host ""
Write-Host "⚠️  IMPORTANT: Please restart your computer to ensure all tools are properly configured." -ForegroundColor Yellow
Write-Host ""
Write-Host "After restarting, you can:" -ForegroundColor White
Write-Host "1. Run .\test-installation.ps1 to verify everything is working" -ForegroundColor Cyan
Write-Host "2. Run .\start-local.ps1 to start DevAssistant" -ForegroundColor Cyan
Write-Host ""
Write-Host "📖 For manual installation instructions, see INSTALLATION.md" -ForegroundColor White

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown') 