# 建置階段
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .
WORKDIR /app/southernTravel
RUN dotnet restore
# 假設您的專案檔名是 southernTravel.csproj
RUN dotnet publish southernTravel.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "southernTravel.dll"]

