FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Aurora.Insurance.Company.Api/Aurora.Insurance.Company.Api.csproj", "src/Aurora.Insurance.Company.Api/"]
COPY ["src/Aurora.Insurance.Company.Domain/Aurora.Insurance.Company.Domain.csproj", "src/Aurora.Insurance.Company.Domain/"]
COPY ["src/Aurora.Insurance.Company.Infrastructure/Aurora.Insurance.Company.Infrastructure.csproj", "src/Aurora.Insurance.Company.Infrastructure/"]
COPY ["src/Aurora.Insurance.Company.Services/Aurora.Insurance.Company.Services.csproj", "src/Aurora.Insurance.Company.Services/"]
COPY ["*.sln", "./"]
COPY ["NuGet.Config", "./"]
RUN dotnet restore
