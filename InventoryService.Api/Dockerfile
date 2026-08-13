FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY InventoryService.Api/InventoryService.Api.csproj InventoryService.Api/
COPY InventoryService.Application/InventoryService.Application.csproj InventoryService.Application/
COPY InventoryService.Domain/InventoryService.Domain.csproj InventoryService.Domain/
COPY InventoryService.Infrastructure/InventoryService.Infrastructure.csproj InventoryService.Infrastructure/

RUN dotnet restore InventoryService.Api/InventoryService.Api.csproj

COPY . .

WORKDIR /src/InventoryService.Api
RUN dotnet publish InventoryService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "InventoryService.Api.dll"]