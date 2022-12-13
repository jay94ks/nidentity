#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

RUN apt-get update -yq && apt-get upgrade -yq && apt-get install -yq curl git nano

COPY . .
RUN dotnet restore "NIdentity/NIdentity.csproj"
WORKDIR "/src/NIdentity"

RUN dotnet build "NIdentity.csproj" -c Release -o /app/build
RUN dotnet publish "NIdentity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "NIdentity.dll"]