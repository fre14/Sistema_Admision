# =============================================
# Multi-stage Dockerfile for SAA.API.Server
# .NET 10 — Production
# =============================================

# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first for layer caching
COPY SAA.Solution.slnx ./
COPY SAA.API/SAA.API.Server/SAA.API.Server.csproj SAA.API/SAA.API.Server/
COPY SAA.Application/SAA.Application.csproj SAA.Application/
COPY SAA.Domain/SAA.Domain.csproj SAA.Domain/
COPY SAA.Infrastructure/SAA.Infrastructure.csproj SAA.Infrastructure/

# Restore dependencies
RUN dotnet restore SAA.API/SAA.API.Server/SAA.API.Server.csproj

# Copy everything else
COPY . .

# Build & publish in Release mode
WORKDIR /src/SAA.API/SAA.API.Server
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

# Copy published output from build stage
COPY --from=build /app/publish .

# Health check
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "SAA.API.Server.dll"]
