# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PathbitChallenge.sln", "."]
COPY ["src/Api/PathbitChallenge.Api.csproj", "src/Api/"]
COPY ["src/Application/PathbitChallenge.Application.csproj", "src/Application/"]
COPY ["src/Domain/PathbitChallenge.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/PathbitChallenge.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Common/PathbitChallenge.Common.csproj", "src/Common/"]
COPY ["tests/UnitTests/PathbitChallenge.UnitTests.csproj", "tests/UnitTests/"]
RUN dotnet restore "PathbitChallenge.sln"
COPY . .
WORKDIR "/src/src/Api"
RUN dotnet build "PathbitChallenge.Api.csproj" -c Release -o /app/build

# Estágio 2: Publish
FROM build AS publish
RUN dotnet publish "PathbitChallenge.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PathbitChallenge.Api.dll"]
