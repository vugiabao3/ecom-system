FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY OrderService.Api/OrderService.Api.csproj OrderService.Api/
COPY OrderService.Application/OrderService.Application.csproj OrderService.Application/
COPY OrderService.Domain/OrderService.Domain.csproj OrderService.Domain/
COPY OrderService.Infrastructure/OrderService.Infrastructure.csproj OrderService.Infrastructure/

RUN dotnet restore OrderService.Api/OrderService.Api.csproj

COPY . .

WORKDIR /src/OrderService.Api
RUN dotnet publish OrderService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "OrderService.Api.dll"]