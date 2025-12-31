FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory-Service.API/Directory-Service.API.csproj", "Directory-Service.API/"]
COPY ["Directory-Service.Application/Directory-Service.Application.csproj", "Directory-Service.Application/"]
COPY ["Directory-Service.Domain/Directory-Service.Domain.csproj", "Directory-Service.Domain/"]
COPY ["Directory-Service.Shared/Directory-Service.Shared.csproj", "Directory-Service.Shared/"]
COPY ["Directory-Service.Contracts/Directory-Service.Contracts.csproj", "Directory-Service.Contracts/"]
RUN dotnet restore "Directory-Service.API/Directory-Service.API.csproj"
COPY . .
WORKDIR "/src/Directory-Service.API"
RUN dotnet build "Directory-Service.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Directory-Service.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Directory-Service.API.dll"]
