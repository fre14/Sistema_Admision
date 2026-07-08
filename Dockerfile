# Dockerfile para SAA.API.Server
# .NET 10 — Produccion con InMemory DB

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar todos los archivos de proyecto
COPY SAA.Domain/SAA.Domain.csproj SAA.Domain/
COPY SAA.Application/SAA.Application.csproj SAA.Application/
COPY SAA.Infrastructure/SAA.Infrastructure.csproj SAA.Infrastructure/
COPY SAA.API/SAA.API.Server/SAA.API.Server.csproj SAA.API/SAA.API.Server/

# Restaurar dependencias
RUN dotnet restore SAA.API/SAA.API.Server/SAA.API.Server.csproj --runtime linux-x64

# Copiar todo el codigo fuente
COPY SAA.Domain/ SAA.Domain/
COPY SAA.Application/ SAA.Application/
COPY SAA.Infrastructure/ SAA.Infrastructure/
COPY SAA.API/SAA.API.Server/ SAA.API/SAA.API.Server/

# Publicar para linux
RUN dotnet publish SAA.API/SAA.API.Server/SAA.API.Server.csproj \
    -c Release \
    -o /app/publish \
    --runtime linux-x64 \
    --self-contained false

# Etapa de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:10000
ENV QuestPDF__Settings__License=Community

EXPOSE 10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "SAA.API.Server.dll"]
