# 🚀 Getting Started with DevAssistant

## ❌ **Current Issue: Missing Required Tools**

Your system is missing the essential tools needed to run DevAssistant. Here's how to fix this:

## 🔧 **Quick Fix (Choose One Option)**

### **Option 1: Auto-Install (Recommended)**
1. **Right-click** on `install-tools.ps1`
2. Select **"Run as Administrator"**
3. Wait for installation to complete
4. **Restart your computer**
5. Run `test-installation.ps1` to verify
6. Run `start-local.ps1` to start DevAssistant

### **Option 2: Manual Installation**
1. **Install .NET 8 SDK**: https://dotnet.microsoft.com/download/dotnet/8.0
2. **Install Node.js 18+**: https://nodejs.org/
3. **Install Ollama** (optional): https://ollama.ai/
4. **Restart your computer**
5. Run `test-installation.ps1` to verify
6. Run `start-local.ps1` to start DevAssistant

### **Option 3: Use Chocolatey Package Manager**
1. **Install Chocolatey** (run in Admin PowerShell):
   ```powershell
   Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))
   ```
2. **Install tools**:
   ```powershell
   choco install dotnet-8.0-sdk
   choco install nodejs
   choco install ollama
   ```
3. **Restart your computer**
4. Run `test-installation.ps1` to verify
5. Run `start-local.ps1` to start DevAssistant

## 📋 **What Each Tool Does**

| Tool | Purpose | Required |
|------|---------|----------|
| **.NET 8 SDK** | Runs the backend API | ✅ **Required** |
| **Node.js 18+** | Runs the Angular frontend | ✅ **Required** |
| **Ollama** | Provides AI chat functionality | ⚠️ **Optional** |
| **SQL Server LocalDB** | Stores conversation history | ⚠️ **Optional** |

## 🧪 **Test Your Installation**

After installing tools, run this to verify everything works:
```powershell
.\test-installation.ps1
```

## 🚀 **Start DevAssistant**

Once all tools are installed:
```powershell
.\start-local.ps1
```

## 🌐 **Access Your App**

- **Frontend**: http://localhost:4200
- **Backend API**: https://localhost:7001
- **Swagger Docs**: https://localhost:7001

## ❌ **Common Errors & Solutions**

| Error | Solution |
|-------|----------|
| `dotnet is not recognized` | Install .NET 8 SDK and restart |
| `node is not recognized` | Install Node.js 18+ and restart |
| `npm is not recognized` | Install Node.js (npm comes with it) |
| Port conflicts | Change ports in config or stop other services |

## 🆘 **Need Help?**

1. **Run the test script**: `.\test-installation.ps1`
2. **Check the logs**: Look for error messages
3. **Read INSTALLATION.md**: Detailed installation guide
4. **Restart your computer**: After installing tools

---

**After installing the required tools, DevAssistant will run perfectly! 🎉** 