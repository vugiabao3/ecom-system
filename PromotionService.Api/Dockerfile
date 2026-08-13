FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PromotionService.Api/PromotionService.Api.csproj PromotionService.Api/
COPY PromotionService.Application/PromotionService.Application.csproj PromotionService.Application/
COPY PromotionService.Domain/PromotionService.Domain.csproj PromotionService.Domain/
COPY PromotionService.Infrastructure/PromotionService.Infrastructure.csproj PromotionService.Infrastructure/

RUN dotnet restore PromotionService.Api/PromotionService.Api.csproj

COPY . .

WORKDIR /src/PromotionService.Api
RUN dotnet publish PromotionService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "PromotionService.Api.dll"]