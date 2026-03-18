FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY FinancialPlanner.slnx ./
COPY FinancialPlanner.Api/FinancialPlanner.Api.csproj FinancialPlanner.Api/
COPY FinancialPlanner.Application/FinancialPlanner.Application.csproj FinancialPlanner.Application/
COPY FinancialPlanner.Domain/FinancialPlanner.Domain.csproj FinancialPlanner.Domain/
COPY FinancialPlanner.Infrastructure/FinancialPlanner.Infrastructure.csproj FinancialPlanner.Infrastructure/
COPY FinancialPlanner.Contracts/FinancialPlanner.Contracts.csproj FinancialPlanner.Contracts/

RUN dotnet restore FinancialPlanner.Api/FinancialPlanner.Api.csproj

COPY . .
RUN dotnet publish FinancialPlanner.Api/FinancialPlanner.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FinancialPlanner.Api.dll"]
