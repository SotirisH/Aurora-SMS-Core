FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
# copy only the cproj files in order to run the Nuget restore
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Aurora.Insurance.Company.Api/Aurora.Insurance.Company.Api.csproj", "src/Aurora.Insurance.Company.Api/"]
COPY ["src/Aurora.Insurance.Company.Domain/Aurora.Insurance.Company.Domain.csproj", "src/Aurora.Insurance.Company.Domain/"]
COPY ["src/Aurora.Insurance.Company.Infrastructure/Aurora.Insurance.Company.Infrastructure.csproj", "src/Aurora.Insurance.Company.Infrastructure/"]
COPY ["src/Aurora.Insurance.Company.Services/Aurora.Insurance.Company.Services.csproj", "src/Aurora.Insurance.Company.Services/"]
COPY ["*.sln", "./"]
COPY ["NuGet.Config", "./"]
# Because we have copied the solution file the restore will run for all files the solution contains
RUN dotnet restore

 # Copy all the files in order to run the build. The output of the build will be in the /app/build
COPY . .
FROM build AS publish
WORKDIR "src/Aurora.Insurance.Company.Api"
RUN dotnet publish "Aurora.Insurance.Company.Api.csproj" --no-restore -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
# Copy all files from the /app/publish of the publish image into current
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aurora.Insurance.Company.Api.dll"]
