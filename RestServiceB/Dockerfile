# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj (with folder path)
COPY ["RestServiceB/RestServiceB.csproj", "RestServiceB/"]
RUN dotnet restore "RestServiceB/RestServiceB.csproj"

# Copy the rest of the project
COPY RestServiceB/ RestServiceB/
WORKDIR "/src/RestServiceB"
RUN dotnet build "RestServiceB.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RestServiceB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestServiceB.dll"]
