FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ProductService.Api/ProductService.Api.csproj ProductService.Api/
COPY ProductService.Application/ProductService.Application.csproj ProductService.Application/
COPY ProductService.Domain/ProductService.Domain.csproj ProductService.Domain/
COPY ProductService.Infrastructure/ProductService.Infrastructure.csproj ProductService.Infrastructure/

RUN dotnet restore ProductService.Api/ProductService.Api.csproj

COPY . .

WORKDIR /src/ProductService.Api
RUN dotnet publish ProductService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProductService.Api.dll"]