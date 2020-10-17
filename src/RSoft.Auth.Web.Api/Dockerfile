#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RSoft.Auth.Web.Api/RSoft.Auth.Web.Api.csproj", "RSoft.Auth.Web.Api/"]
COPY ["RSoft.Auth.Application/RSoft.Auth.Application.csproj", "RSoft.Auth.Application/"]
COPY ["RSoft.Auth.Domain/RSoft.Auth.Domain.csproj", "RSoft.Auth.Domain/"]
COPY ["RSoft.Auth.Cross.Common/RSoft.Auth.Cross.Common.csproj", "RSoft.Auth.Cross.Common/"]
COPY ["RSoft.Auth.Cross.IoC/RSoft.Auth.Cross.IoC.csproj", "RSoft.Auth.Cross.IoC/"]
COPY ["RSoft.Auth.Infra.Data/RSoft.Auth.Infra.Data.csproj", "RSoft.Auth.Infra.Data/"]
RUN dotnet restore "RSoft.Auth.Web.Api/RSoft.Auth.Web.Api.csproj"
COPY . .
WORKDIR "/src/RSoft.Auth.Web.Api"
RUN dotnet build "RSoft.Auth.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RSoft.Auth.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RSoft.Auth.Web.Api.dll"]