#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

RUN apt-get update && apt-get -y install libgdiplus libc6-dev

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["cosmosdbviewer/cosmosdbviewer.csproj", "cosmosdbviewer/"]
RUN dotnet restore "cosmosdbviewer/cosmosdbviewer.csproj"
COPY . .
WORKDIR "/src/cosmosdbviewer"
RUN dotnet build "cosmosdbviewer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cosmosdbviewer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "cosmosdbviewer.dll"]