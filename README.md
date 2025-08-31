# 🚀 DevAssistant - AI-Powered Developer Helper

A modern, full-stack web application that provides AI-powered development assistance using local LLMs through Ollama. Built with .NET 8 Web API and Angular 17.

## ✨ Features

- **🤖 AI-Powered Assistance**: Get help with .NET, C#, ASP.NET Core, Entity Framework, and SQL Server
- **📝 Smart Templates**: Pre-built prompts for error explanation, code refactoring, SQL help, and more
- **⚡ Real-time Streaming**: ChatGPT-like streaming responses using Server-Sent Events
- **💬 Chat Interface**: Modern, responsive chat UI with message history
- **📋 Code Formatting**: Automatic code block detection with syntax highlighting and copy-to-clipboard
- **👍 Feedback System**: Rate responses to improve AI quality
- **🔒 Local LLM**: Uses Ollama for privacy and offline capability
- **🐳 Docker Ready**: Complete containerization for easy deployment

## 🏗 Architecture

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular 17    │    │  .NET 8 Web API │    │     Ollama      │
│   Frontend      │◄──►│     Backend     │◄──►│   Local LLM     │
│                 │    │                 │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Bootstrap 5   │    │ Entity Framework│    │   SQL Server    │
│   Responsive UI │    │   Code First    │    │   Database      │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Ollama](https://ollama.ai/) (for local LLM)

### Option 1: Docker Compose (Recommended)

1. **Clone the repository**
   ```bash
   git clone <your-repo-url>
   cd dotnet_llm
   ```

2. **Start all services**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:7001
   - Swagger UI: http://localhost:7001
   - SQL Server: localhost,1433

4. **Pull Ollama models** (first time)
   ```bash
   docker exec -it dotnet_llm-ollama-1 ollama pull mistral
   docker exec -it dotnet_llm-ollama-1 ollama pull codellama
   ```

### Option 2: Local Development

1. **Start Ollama locally**
   ```bash
   ollama serve
   ollama pull mistral
   ```

2. **Run the backend**
   ```bash
   cd DevAssistant.Api
   dotnet run
   ```

3. **Run the frontend**
   ```bash
   cd DevAssistant.Frontend
   npm install
   npm start
   ```

## 🎯 Available Templates

| Template | Key | Description |
|----------|-----|-------------|
| **General Assistant** | `general` | General development questions and guidance |
| **Error Explainer** | `error-explain` | Analyze and explain errors with solutions |
| **Code Refactoring** | `refactor` | Improve code quality and maintainability |
| **SQL Helper** | `sql-helper` | SQL Server queries and database operations |
| **Code Review** | `code-review` | Comprehensive code analysis and suggestions |
| **Unit Test Generator** | `unit-test` | Generate comprehensive unit tests |

## 🔧 Configuration

### Backend Configuration (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DevAssistantDb;Trusted_Connection=true;"
  },
  "Ollama": {
    "BaseUrl": "http://localhost:11434",
    "DefaultModel": "mistral",
    "RequestTimeoutMinutes": 5
  }
}
```

### Frontend Configuration

The frontend automatically connects to the backend at `https://localhost:7001`. Update the `apiBaseUrl` in `chat.service.ts` if needed.

## 📚 API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `POST /api/chat/stream` | POST | Stream chat responses using SSE |
| `GET /api/chat/templates` | GET | Get available prompt templates |
| `GET /api/chat/health` | GET | Check Ollama connection status |
| `GET /health` | GET | Application health check |

## 🎨 UI Features

- **Responsive Design**: Works on desktop, tablet, and mobile
- **Dark Code Theme**: Syntax highlighting for code blocks
- **Auto-scroll**: Automatically scrolls to new messages
- **Typing Indicators**: Shows when AI is processing
- **Template Selection**: Dropdown for choosing prompt templates
- **Feedback System**: Thumbs up/down for responses
- **Message History**: Persistent conversation storage

## 🐳 Docker Services

- **Frontend**: Nginx serving Angular app on port 4200
- **Backend**: .NET 8 API on ports 7001/7002
- **Database**: SQL Server 2022 on port 1433
- **Ollama**: Local LLM service on port 11434

## 🔍 Troubleshooting

### Ollama Connection Issues

1. **Check if Ollama is running**
   ```bash
   curl http://localhost:11434/api/tags
   ```

2. **Verify model availability**
   ```bash
   ollama list
   ```

3. **Pull required models**
   ```bash
   ollama pull mistral
   ollama pull codellama
   ```

### Database Connection Issues

1. **Check SQL Server status**
   ```bash
   docker logs dotnet_llm-sqlserver-1
   ```

2. **Verify connection string**
   - Check `appsettings.json`
   - Ensure SQL Server is accessible

### Frontend Issues

1. **Check API connectivity**
   - Verify backend is running
   - Check CORS configuration
   - Review browser console for errors

## 🚀 Deployment

### Production Deployment

1. **Update configuration**
   - Set production connection strings
   - Configure HTTPS certificates
   - Update API base URLs

2. **Build and deploy**
   ```bash
   docker-compose -f docker-compose.prod.yml up -d
   ```

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | .NET environment | `Development` |
| `ConnectionStrings__DefaultConnection` | Database connection | LocalDB |
| `Ollama__BaseUrl` | Ollama service URL | `http://localhost:11434` |

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [Ollama](https://ollama.ai/) for local LLM capabilities
- [.NET 8](https://dotnet.microsoft.com/) for the backend framework
- [Angular 17](https://angular.io/) for the frontend framework
- [Bootstrap 5](https://getbootstrap.com/) for responsive UI components

## 📞 Support

- **Issues**: Create a GitHub issue
- **Discussions**: Use GitHub Discussions
- **Documentation**: Check the Wiki

---

**Happy Coding! 🎉** 