# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY FrightNight.csproj ./
COPY FrightNight.sln ./
RUN dotnet restore
COPY . .
RUN dotnet restore --verbosity detailed
RUN dotnet publish FrightNight.csproj -c Release -o /app/publish --verbosity detailed

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 10000
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "FrightNight.dll"]