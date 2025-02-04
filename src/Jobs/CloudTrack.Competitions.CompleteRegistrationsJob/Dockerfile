# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Shared/Directory.Build.props", "src/Shared/"]
COPY ["src/Jobs/CloudTrack.Competitions.CompleteRegistrationsJob/CloudTrack.Competitions.CompleteRegistrationsJob.csproj", "src/Jobs/CloudTrack.Competitions.CompleteRegistrationsJob/"]
COPY ["src/CloudTrack.Competitions.Application/CloudTrack.Competitions.Application.csproj", "src/CloudTrack.Competitions.Application/"]
COPY ["src/CloudTrack.Competitions.Domain/CloudTrack.Competitions.Domain.csproj", "src/CloudTrack.Competitions.Domain/"]
COPY ["src/Shared/CloudTrack.Competitions.Messaging/CloudTrack.Competitions.Messaging.csproj", "src/Shared/CloudTrack.Competitions.Messaging/"]
COPY ["src/CloudTrack.Competitions.Infrastructure/CloudTrack.Competitions.Infrastructure.csproj", "src/CloudTrack.Competitions.Infrastructure/"]
RUN dotnet restore "./src/Jobs/CloudTrack.Competitions.CompleteRegistrationsJob/CloudTrack.Competitions.CompleteRegistrationsJob.csproj"
COPY . .
WORKDIR "/src/src/Jobs/CloudTrack.Competitions.CompleteRegistrationsJob"
RUN dotnet build "./CloudTrack.Competitions.CompleteRegistrationsJob.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CloudTrack.Competitions.CompleteRegistrationsJob.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudTrack.Competitions.CompleteRegistrationsJob.dll"]