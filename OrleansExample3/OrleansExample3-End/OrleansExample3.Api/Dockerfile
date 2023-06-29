FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 11111
EXPOSE 30000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Silo/Orleans.ShoppingCart.Silo.csproj", "Silo/"]
RUN dotnet restore "Silo/Orleans.ShoppingCart.Silo.csproj"
COPY . .
WORKDIR "/src/Silo"
RUN dotnet build "Orleans.ShoppingCart.Silo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orleans.ShoppingCart.Silo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orleans.ShoppingCart.Silo.dll"]
