#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RSoft.Auth.RecoveryAccess.Worker/RSoft.Auth.RecoveryAccess.Worker.csproj", "RSoft.Auth.RecoveryAccess.Worker/"]
COPY ["RSoft.Auth.Application/RSoft.Auth.Application.csproj", "RSoft.Auth.Application/"]
COPY ["RSoft.Auth.Cross.Common/RSoft.Auth.Cross.Common.csproj", "RSoft.Auth.Cross.Common/"]
COPY ["RSoft.Auth.Domain/RSoft.Auth.Domain.csproj", "RSoft.Auth.Domain/"]
COPY ["RSoft.Auth.Cross.IoC/RSoft.Auth.Cross.IoC.csproj", "RSoft.Auth.Cross.IoC/"]
COPY ["RSoft.Auth.Infra.Data/RSoft.Auth.Infra.Data.csproj", "RSoft.Auth.Infra.Data/"]
RUN dotnet restore "RSoft.Auth.RecoveryAccess.Worker/RSoft.Auth.RecoveryAccess.Worker.csproj"
COPY . .
WORKDIR "/src/RSoft.Auth.RecoveryAccess.Worker"
RUN dotnet build "RSoft.Auth.RecoveryAccess.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RSoft.Auth.RecoveryAccess.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RSoft.Auth.RecoveryAccess.Worker.dll"]