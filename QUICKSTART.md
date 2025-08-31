# 🚀 DevAssistant Quick Start Guide

Get your AI-powered developer assistant running in 5 minutes!

## ⚡ Quick Start (Docker)

### 1. Start Everything
```bash
# Start all services
docker-compose up -d

# Check status
docker-compose ps
```

### 2. Pull AI Models
```bash
# Pull Mistral model (recommended)
docker exec -it dotnet_llm-ollama-1 ollama pull mistral

# Or pull CodeLlama for coding
docker exec -it dotnet_llm-ollama-1 ollama pull codellama
```

### 3. Open DevAssistant
- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:7001
- **Swagger Docs**: http://localhost:7001

## 🎯 Try It Out

1. **Open** http://localhost:4200
2. **Select a template** (Error Explain, Refactor, SQL Helper)
3. **Ask a question** like:
   - "How do I create a Web API controller in .NET 8?"
   - "Explain this error: 'Object reference not set to an instance of an object'"
   - "Help me refactor this C# code for better performance"

## 🔧 Troubleshooting

### Ollama Not Responding?
```bash
# Check if Ollama is running
docker logs dotnet_llm-ollama-1

# Restart Ollama service
docker-compose restart ollama
```

### Backend Not Working?
```bash
# Check backend logs
docker logs dotnet_llm-backend-1

# Restart backend
docker-compose restart backend
```

### Frontend Issues?
```bash
# Check frontend logs
docker logs dotnet_llm-frontend-1

# Restart frontend
docker-compose restart frontend
```

## 📱 Available Templates

| Template | Use For |
|----------|---------|
| **General** | General .NET questions |
| **Error Explain** | Debugging and error analysis |
| **Refactor** | Code improvement suggestions |
| **SQL Helper** | Database and SQL questions |
| **Code Review** | Code quality analysis |
| **Unit Test** | Test generation |

## 🚀 Next Steps

- **Customize prompts** in `DevAssistant.Api/Services/PromptTemplateService.cs`
- **Add new models** by pulling them in Ollama
- **Deploy to production** using `docker-compose.prod.yml`
- **Monitor usage** through the database logs

## 💡 Pro Tips

- Use **Shift+Enter** for new lines in chat
- **Copy code blocks** with the copy button
- **Rate responses** with 👍/👎 to improve AI quality
- **Clear chat** to start fresh conversations

---

**Need help?** Check the main README.md or run `.\test-setup.ps1` to diagnose issues! 