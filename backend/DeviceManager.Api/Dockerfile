# Place this file at: backend/DeviceManager.Api/Dockerfile
# Build context (in docker-compose) is the repo root, so paths below are repo-relative.

# ---- Stage 1: build ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj first for better layer caching
COPY backend/DeviceManager.Api/DeviceManager.Api.csproj backend/DeviceManager.Api/
RUN dotnet restore backend/DeviceManager.Api/DeviceManager.Api.csproj

# Copy the rest and publish
COPY backend/DeviceManager.Api/ backend/DeviceManager.Api/
RUN dotnet publish backend/DeviceManager.Api/DeviceManager.Api.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# ---- Stage 2: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Listen on 8080 inside the container (no root needed)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DeviceManager.Api.dll"]
