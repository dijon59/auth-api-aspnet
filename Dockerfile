# Use official .NET SDK for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy project files
COPY . .

# Restore dependencies
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /out

# Runtime image (lighter)
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /out .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BackendApi.dll"]
