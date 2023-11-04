FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./Building Blocks/NSE.Core/NSE.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.WebAPI.Core/NSE.WebAPI.Core.csproj", "Building Blocks/NSE.WebAPI.Core/"]
COPY ["./Building Blocks/NSE.MessageBus/NSE.MessageBus/NSE.MessageBus.csproj", "Building Blocks/NSE.MessageBus/NSE.MessageBus/"]
COPY ["./Services/NSE.Pagamentos.NerdsPag/NSE.Pagamentos.NerdsPag/NSE.Pagamentos.NerdsPag.csproj", "Services/NSE.Pagamentos.NerdsPag/NSE.Pagamentos.NerdsPag/"]

RUN dotnet restore "Services/NSE.Pagamento.API/NSE.Pagamento.API.csproj"
COPY . .
WORKDIR "Services/NSE.Pagamento.API"
RUN dotnet build "NSE.Pagamento.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSE.Pagamento.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSE.Pagamento.API.dll"]