
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/services/Reports/MicroserviceSample.Services.Reports.API"
FROM build AS publish
RUN dotnet publish -c Release -r linux-x64 --no-self-contained  -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceSample.Services.Reports.API.dll"]