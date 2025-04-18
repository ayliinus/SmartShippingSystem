FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SmartShipping.API/SmartShipping.API.csproj", "SmartShipping.API/"]
COPY ["SmartShipping.Application/SmartShipping.Application.csproj", "SmartShipping.Application/"]
COPY ["SmartShipping.Domain/SmartShipping.Domain.csproj", "SmartShipping.Domain/"]
COPY ["SmartShipping.Persistence/SmartShipping.Persistence.csproj", "SmartShipping.Persistence/"]
COPY ["SmartShipping.Infrastructure/SmartShipping.Infrastructure.csproj", "SmartShipping.Infrastructure/"]
COPY ["SmartShipping.Shared/SmartShipping.Shared.csproj", "SmartShipping.Shared/"]

RUN dotnet restore "SmartShipping.API/SmartShipping.API.csproj"

COPY . .
WORKDIR "/src/SmartShipping.API"
RUN dotnet build "SmartShipping.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartShipping.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartShipping.API.dll"]
