# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Watchlist Tracker.csproj", "./"]
RUN dotnet restore "Watchlist Tracker.csproj"

COPY . .
RUN dotnet build "Watchlist Tracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Watchlist Tracker.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 5000 5001

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Watchlist Tracker.dll"]
