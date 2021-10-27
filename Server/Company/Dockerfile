FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Server/Company/Aurora.Insurance.Company.Api/Aurora.Insurance.Company.Api.csproj", "build-src/"]
RUN dotnet restore "build-src/Aurora.Insurance.Company.Api.csproj"
COPY . .
WORKDIR "/build-src"
RUN dotnet build "Aurora.Insurance.Company.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aurora.Insurance.Company.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aurora.Insurance.Company.Api.dll"]
