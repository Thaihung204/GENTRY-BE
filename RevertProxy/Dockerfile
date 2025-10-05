# Base image to run ASP.NET Core apps
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080  

# SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore
COPY ["ReverseProxy.csproj", "."]
RUN dotnet restore

# Copy all source and build
COPY . .
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ReverseProxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ✅ Copy cấu hình appsettings.json (và các file liên quan)
COPY appsettings*.json /app/publish

# Final runtime image
FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ReverseProxy.dll"]
