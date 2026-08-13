FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CartService.Api/CartService.Api.csproj CartService.Api/
COPY CartService.Application/CartService.Application.csproj CartService.Application/
COPY CartService.Domain/CartService.Domain.csproj CartService.Domain/
COPY CartService.Infrastructure/CartService.Infrastructure.csproj CartService.Infrastructure/

RUN dotnet restore CartService.Api/CartService.Api.csproj

COPY . .

WORKDIR /src/CartService.Api
RUN dotnet publish CartService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CartService.Api.dll"]