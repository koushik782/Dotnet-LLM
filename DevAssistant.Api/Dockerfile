FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DevAssistant.Api.csproj", "./"]
RUN dotnet restore "DevAssistant.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "DevAssistant.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevAssistant.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevAssistant.Api.dll"] 