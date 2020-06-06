# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY *.sln .
# app
COPY src/Application/Modulbank.App/*.csproj src/Application/Modulbank.App/
COPY src/Application/Modulbank.App.Api/*.csproj src/Application/Modulbank.App.Api/
COPY src/Application/Modulbank.MessageHandlers/*.csproj src/Application/Modulbank.MessageHandlers/
# core
COPY src/Core/Modulbank.Accounts/*.csproj src/Core/Modulbank.Accounts/
COPY src/Core/Modulbank.Data/*.csproj src/Core/Modulbank.Data/
COPY src/Core/Modulbank.FileStorage/*.csproj src/Core/Modulbank.FileStorage/
COPY src/Core/Modulbank.Profiles/*.csproj src/Core/Modulbank.Profiles/
COPY src/Core/Modulbank.Rebus/*.csproj src/Core/Modulbank.Rebus/
COPY src/Core/Modulbank.Shared/*.csproj src/Core/Modulbank.Shared/
COPY src/Core/Modulbank.Users/*.csproj src/Core/Modulbank.Users/
# infrastructure
COPY src/Infrastructure/Modulbank.Packages/*.csproj src/Infrastructure/Modulbank.Packages/
COPY src/Infrastructure/Modulbank.Settings/*.csproj src/Infrastructure/Modulbank.Settings/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /app/src/Application/Modulbank.App.Api
RUN dotnet publish -c Release -o /app/publish

# runtime

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_ENVIRONMENT=Production
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Modulbank.App.Api.dll