@echo off
echo 🚀 Starting DevAssistant Local Development...
echo.

echo 1. Starting Backend API...
start "DevAssistant Backend" cmd /k "cd /d %~dp0DevAssistant.Api && dotnet run"

echo 2. Starting Frontend...
start "DevAssistant Frontend" cmd /k "cd /d %~dp0DevAssistant.Frontend && npm install && npm start"

echo.
echo ✅ DevAssistant is starting up!
echo.
echo 📱 Frontend: http://localhost:4200
echo 🔧 Backend API: https://localhost:7001
echo 📚 Swagger UI: https://localhost:7001
echo.
echo ⏳ Please wait a few minutes for both services to fully start...
echo.
pause
start http://localhost:4200 