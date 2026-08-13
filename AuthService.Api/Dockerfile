FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AuthService.Api/AuthService.Api.csproj AuthService.Api/
COPY AuthService.Application/AuthService.Application.csproj AuthService.Application/
COPY AuthService.Domain/AuthService.Domain.csproj AuthService.Domain/
COPY AuthService.Infrastructure/AuthService.Infrastructure.csproj AuthService.Infrastructure/

RUN dotnet restore AuthService.Api/AuthService.Api.csproj

COPY . .

WORKDIR /src/AuthService.Api
RUN dotnet publish AuthService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]