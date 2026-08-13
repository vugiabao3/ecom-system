FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY PaymentService.Api/PaymentService.Api.csproj PaymentService.Api/
COPY PaymentService.Application/PaymentService.Application.csproj PaymentService.Application/
COPY PaymentService.Domain/PaymentService.Domain.csproj PaymentService.Domain/
COPY PaymentService.Infrastructure/PaymentService.Infrastructure.csproj PaymentService.Infrastructure/

RUN dotnet restore PaymentService.Api/PaymentService.Api.csproj

COPY . .

WORKDIR /src/PaymentService.Api
RUN dotnet publish PaymentService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "PaymentService.Api.dll"]