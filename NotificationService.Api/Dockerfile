FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY NotificationService.Api/NotificationService.Api.csproj NotificationService.Api/
COPY NotificationService.Application/NotificationService.Application.csproj NotificationService.Application/
COPY NotificationService.Domain/NotificationService.Domain.csproj NotificationService.Domain/
COPY NotificationService.Infrastructure/NotificationService.Infrastructure.csproj NotificationService.Infrastructure/

RUN dotnet restore NotificationService.Api/NotificationService.Api.csproj

COPY . .

WORKDIR /src/NotificationService.Api
RUN dotnet publish NotificationService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]