FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ShippingService.Api/ShippingService.Api.csproj ShippingService.Api/
COPY ShippingService.Application/ShippingService.Application.csproj ShippingService.Application/
COPY ShippingService.Domain/ShippingService.Domain.csproj ShippingService.Domain/
COPY ShippingService.Infrastructure/ShippingService.Infrastructure.csproj ShippingService.Infrastructure/

RUN dotnet restore ShippingService.Api/ShippingService.Api.csproj

COPY . .

WORKDIR /src/ShippingService.Api
RUN dotnet publish ShippingService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ShippingService.Api.dll"]