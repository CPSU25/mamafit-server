FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files (bắt buộc cho multi-project!)
COPY ["MamaFit.sln", "."]
COPY ["MamaFit.API/MamaFit.API.csproj", "MamaFit.API/"]
COPY ["MamaFit.BusinessObjects/MamaFit.BusinessObjects.csproj", "MamaFit.BusinessObjects/"]
COPY ["MamaFit.Repositories/MamaFit.Repositories.csproj", "MamaFit.Repositories/"]
COPY ["MamaFit.Services/MamaFit.Services.csproj", "MamaFit.Services/"]

# Restore nuget package
RUN dotnet restore "./MamaFit.API/MamaFit.API.csproj"

# Copy toàn bộ source code còn lại
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
