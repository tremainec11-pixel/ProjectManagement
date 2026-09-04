# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project file
COPY ProjectManagement.API/ProjectManagement.API.csproj ProjectManagement.API/

# Restore dependencies
RUN dotnet restore ProjectManagement.API/ProjectManagement.API.csproj

# Copy source code
COPY ProjectManagement.API/ ProjectManagement.API/

# Build and publish
RUN dotnet publish ProjectManagement.API/ProjectManagement.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Render provides the PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "ProjectManagement.API.dll"]