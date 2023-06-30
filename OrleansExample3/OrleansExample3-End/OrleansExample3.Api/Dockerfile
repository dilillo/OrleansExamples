FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 11111
EXPOSE 30000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["OrleansExample3.Api/OrleansExample3.Api.csproj", "Silo/"]
RUN dotnet restore "Silo/OrleansExample3.Api.csproj"
COPY . .
WORKDIR "/src/Silo"
RUN dotnet build "OrleansExample3.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrleansExample3.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrleansExample3.Api.dll"]
