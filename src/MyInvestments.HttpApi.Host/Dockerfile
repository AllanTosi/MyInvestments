#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
#EXPOSE 44363
#EXPOSE 44377
#EXPOSE 44348

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MyInvestments.HttpApi.Host/MyInvestments.HttpApi.Host.csproj", "src/MyInvestments.HttpApi.Host/"]
COPY ["src/MyInvestments.Application/MyInvestments.Application.csproj", "src/MyInvestments.Application/"]
COPY ["src/MyInvestments.Domain/MyInvestments.Domain.csproj", "src/MyInvestments.Domain/"]
COPY ["src/MyInvestments.Domain.Shared/MyInvestments.Domain.Shared.csproj", "src/MyInvestments.Domain.Shared/"]
COPY ["src/MyInvestments.Application.Contracts/MyInvestments.Application.Contracts.csproj", "src/MyInvestments.Application.Contracts/"]
COPY ["src/MyInvestments.EntityFrameworkCore/MyInvestments.EntityFrameworkCore.csproj", "src/MyInvestments.EntityFrameworkCore/"]
COPY ["src/MyInvestments.HttpApi/MyInvestments.HttpApi.csproj", "src/MyInvestments.HttpApi/"]
RUN dotnet restore "./src/MyInvestments.HttpApi.Host/MyInvestments.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/MyInvestments.HttpApi.Host"
RUN dotnet build "./MyInvestments.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyInvestments.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyInvestments.HttpApi.Host.dll"]