#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataTransfer.Api/DataTransfer.Api.csproj", "DataTransfer.Api/"]
RUN dotnet restore "DataTransfer.Api/DataTransfer.Api.csproj"
COPY . .
WORKDIR "/src/DataTransfer.Api"
RUN dotnet build "DataTransfer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataTransfer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataTransfer.Api.dll"]