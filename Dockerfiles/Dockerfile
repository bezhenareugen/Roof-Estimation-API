FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj files
COPY ["RoofEstimation.Api/RoofEstimation.Api.csproj", "RoofEstimation.Api/"]
COPY ["RoofEstimation.DAL/RoofEstimation.DAL.csproj", "RoofEstimation.DAL/"]
COPY ["RoofEstimation.BLL/RoofEstimation.BLL.csproj", "RoofEstimation.BLL/"]
COPY ["RoofEstimation.Entities/RoofEstimation.Entities.csproj", "RoofEstimation.Entities/"]
COPY ["RoofEstimation.Models/RoofEstimation.Models.csproj", "RoofEstimation.Models/"]

RUN dotnet restore "RoofEstimation.Api/RoofEstimation.Api.csproj"

# Copy everything
COPY . .

WORKDIR "/src/RoofEstimation.Api"
RUN dotnet build "RoofEstimation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RoofEstimation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoofEstimation.Api.dll"]
