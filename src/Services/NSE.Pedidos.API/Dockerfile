FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Building Blocks/NSE.Core/NSE.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.MessageBus/NSE.MessageBus/NSE.MessageBus.csproj", "Building Blocks/NSE.MessageBus/NSE.MessageBus/"]
COPY ["./Services/NSE.Pedidos.Domain/NSE.Pedidos.Domain.csproj", "Services/NSE.Pedidos.Domain/"]
COPY ["./Services/NSE.Pedidos.Infra/NSE.Pedidos.Infra.csproj", "Services/NSE.Pedidos.Infra/"]

RUN dotnet restore "Services/NSE.Pedidos.API/NSE.Pedidos.API.csproj"
COPY . .
WORKDIR "Services/NSE.Pedidos.API"
RUN dotnet build "NSE.Pedidos.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Pedidos.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Pedidos.API.dll"]