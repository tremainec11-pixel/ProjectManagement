# =========================
# Build Stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution
COPY ProjectManagement.sln ./

# Copy project files
COPY ProjectManagement.API/ProjectManagement.API.csproj ProjectManagement.API/
COPY ProjectManagement.Application/ProjectManagement.Application.csproj ProjectManagement.Application/
COPY ProjectManagement.Domain/ProjectManagement.Domain.csproj ProjectManagement.Domain/
COPY ProjectManagement.Infrastructure/ProjectManagement.Infrastructure.csproj ProjectManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore ProjectManagement.API/ProjectManagement.API.csproj

# Copy the rest of the source code
COPY . .

# Build and publish
RUN dotnet publish ProjectManagement.API/ProjectManagement.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# =========================
# Runtime Stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

# Render provides the PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT}

ENTRYPOINT ["dotnet", "ProjectManagement.API.dll"]