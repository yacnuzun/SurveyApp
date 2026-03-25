FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["services/SurveyApp.ReportingService/SurveyApp.ReportingService/SurveyApp.ReportingService.csproj", "services/SurveyApp.ReportingService/SurveyApp.ReportingService/"]
COPY ["shared/SurveyApp.Shared/SurveyApp.Shared.csproj", "shared/SurveyApp.Shared/"]
RUN dotnet restore "services/SurveyApp.ReportingService/SurveyApp.ReportingService/SurveyApp.ReportingService.csproj"

COPY . .
WORKDIR "/src/services/SurveyApp.ReportingService/SurveyApp.ReportingService"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyApp.ReportingService.dll"]