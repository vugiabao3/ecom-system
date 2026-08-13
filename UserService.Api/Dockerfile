FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY UserService.Api/UserService.Api.csproj UserService.Api/
COPY UserService.Application/UserService.Application.csproj UserService.Application/
COPY UserService.Domain/UserService.Domain.csproj UserService.Domain/
COPY UserService.Infrastructure/UserService.Infrastructure.csproj UserService.Infrastructure/
COPY Contracts/EcomSystem.Contracts.csproj Contracts/

RUN dotnet restore UserService.Api/UserService.Api.csproj

COPY . .

WORKDIR /src/UserService.Api
RUN dotnet publish UserService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "UserService.Api.dll"]