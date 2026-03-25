# SurveyApp/backend/services/SurveyApp.IdentityService/Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["services/SurveyApp.IdentityService/SurveyApp.IdentityService.csproj", "services/SurveyApp.IdentityService/"]
COPY ["shared/SurveyApp.Shared/SurveyApp.Shared.csproj", "shared/SurveyApp.Shared/"]
RUN dotnet restore "services/SurveyApp.IdentityService/SurveyApp.IdentityService.csproj"

COPY . .
WORKDIR "/src/services/SurveyApp.IdentityService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyApp.IdentityService.dll"]
