#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/services/Contacts/MicroserviceSample.Services.Contacts.API/MicroserviceSample.Services.Contacts.API.csproj", "src/services/Contacts/MicroserviceSample.Services.Contacts.API/"]
COPY ["src/building-blocks/MicroserviceSample.BuildingBlocks.Application/MicroserviceSample.BuildingBlocks.Application.csproj", "src/building-blocks/MicroserviceSample.BuildingBlocks.Application/"]
COPY ["src/services/Contacts/MicroserviceSample.Services.Contacts.Application/MicroserviceSample.Services.Contacts.Application.csproj", "src/services/Contacts/MicroserviceSample.Services.Contacts.Application/"]
COPY ["src/services/Contacts/MicroserviceSample.Services.Contacts.Domain/MicroserviceSample.Services.Contacts.Domain.csproj", "src/services/Contacts/MicroserviceSample.Services.Contacts.Domain/"]
COPY ["src/building-blocks/MicroserviceSample.BuildingBlocks.Domain/MicroserviceSample.BuildingBlocks.Domain.csproj", "src/building-blocks/MicroserviceSample.BuildingBlocks.Domain/"]
COPY ["src/services/Contacts/MicroserviceSample.Services.Contacts.Infrastructure/MicroserviceSample.Services.Contacts.Infrastructure.csproj", "src/services/Contacts/MicroserviceSample.Services.Contacts.Infrastructure/"]
COPY ["src/building-blocks/MicroserviceSample.BuildingBlocks.Infrastructure/MicroserviceSample.BuildingBlocks.Infrastructure.csproj", "src/building-blocks/MicroserviceSample.BuildingBlocks.Infrastructure/"]
RUN dotnet restore "src/services/Contacts/MicroserviceSample.Services.Contacts.API/MicroserviceSample.Services.Contacts.API.csproj"
COPY . .
WORKDIR "/src/src/services/Contacts/MicroserviceSample.Services.Contacts.API"
RUN dotnet build "MicroserviceSample.Services.Contacts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceSample.Services.Contacts.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceSample.Services.Contacts.API.dll"]