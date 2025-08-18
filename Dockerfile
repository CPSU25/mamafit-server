FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Kestrel lắng nghe HTTP 8080 trong container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Cài curl cho healthcheck (Debian-based)
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

# Healthcheck: 200 OK nếu API chạy
HEALTHCHECK --interval=30s --timeout=5s --retries=5 \
  CMD curl -fsS http://localhost:8080/health || exit 1


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution và project files
COPY ["MamaFit.sln", "."]
COPY ["MamaFit.API/MamaFit.API.csproj", "MamaFit.API/"]
COPY ["MamaFit.BusinessObjects/MamaFit.BusinessObjects.csproj", "MamaFit.BusinessObjects/"]
COPY ["MamaFit.Repositories/MamaFit.Repositories.csproj", "MamaFit.Repositories/"]
COPY ["MamaFit.Services/MamaFit.Services.csproj", "MamaFit.Services/"]

# Restore nuget
RUN dotnet restore "./MamaFit.API/MamaFit.API.csproj"

# Copy source còn lại
COPY . .

WORKDIR "/src/MamaFit.API"
RUN dotnet build "./MamaFit.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MamaFit.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MamaFit.API.dll"]
