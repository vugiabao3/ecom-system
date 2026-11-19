FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ReviewService.Api/ReviewService.Api.csproj ReviewService.Api/
COPY ReviewService.Application/ReviewService.Application.csproj ReviewService.Application/
COPY ReviewService.Domain/ReviewService.Domain.csproj ReviewService.Domain/
COPY ReviewService.Infrastructure/ReviewService.Infrastructure.csproj ReviewService.Infrastructure/

RUN dotnet restore ReviewService.Api/ReviewService.Api.csproj

COPY . .

WORKDIR /src/ReviewService.Api
RUN dotnet publish ReviewService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ReviewService.Api.dll"]
