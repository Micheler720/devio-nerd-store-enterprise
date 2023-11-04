FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Services/NSE.Cliente.API/NSE.Cliente.API.csproj", "Services/NSE.Cliente.API/"]
COPY ["./Building Blocks/NSE.Core/NSE.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.MessageBus/NSE.MessageBus/NSE.MessageBus.csproj", "Building Blocks/NSE.MessageBus/NSE.MessageBus/"]

RUN dotnet restore "Services/NSE.Cliente.API/NSE.Cliente.API.csproj"
COPY . .
WORKDIR "Services/NSE.Cliente.API"
RUN dotnet build "NSE.Cliente.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Cliente.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Cliente.API.dll"]