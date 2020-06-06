# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY *.sln .
COPY src/* src/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/Application/Modulbank.App.Api
RUN dotnet publish -c Release -o /src/publish

# runtime
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Modulbank.App.Api.dll