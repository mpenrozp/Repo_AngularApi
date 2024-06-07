FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY WebApiProducto.csproj .
RUN dotnet restore
COPY . .
EXPOSE 8181
RUN dotnet build "WebApiProducto.csproj" -c Release -o /app/build

RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "WebApiProducto.dll"]
