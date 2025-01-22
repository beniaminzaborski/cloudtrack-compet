#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/CloudTrack.Competitions.WebAPI/CloudTrack.Competitions.WebAPI.csproj", "src/CloudTrack.Competitions.WebAPI/"]
COPY ["src/CloudTrack.Competitions.Application/CloudTrack.Competitions.Application.csproj", "src/CloudTrack.Competitions.Application/"]
COPY ["src/CloudTrack.Competitions.Domain/CloudTrack.Competitions.Domain.csproj", "src/CloudTrack.Competitions.Domain/"]
COPY ["src/CloudTrack.Competitions.Infrastructure/CloudTrack.Competitions.Infrastructure.csproj", "src/CloudTrack.Competitions.Infrastructure/"]
RUN dotnet restore "src/CloudTrack.Competitions.WebAPI/CloudTrack.Competitions.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/CloudTrack.Competitions.WebAPI"
RUN dotnet build "CloudTrack.Competitions.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudTrack.Competitions.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudTrack.Competitions.WebAPI.dll"]