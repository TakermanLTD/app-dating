FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT Production
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install -y curl gnupg
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -fsSL https://deb.nodesource.com/nsolid_setup_deb.sh | sh -s 20
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update
RUN apt-get install -y curl
RUN apt-get install -y libpng-dev libjpeg-dev curl libxi6 build-essential libgl1-mesa-glx
RUN curl -fsSL https://deb.nodesource.com/nsolid_setup_deb.sh | sh -s 20
RUN apt-get install -y nodejs
ARG BUILD_CONFIGURATION=Release

WORKDIR /app
COPY Takerman.Dating.Data/*.csproj ./Takerman.Dating.Data/
WORKDIR /app/Takerman.Dating.Data/
RUN dotnet restore

WORKDIR /app
COPY Takerman.Dating.Services/*.csproj ./Takerman.Dating.Services/
WORKDIR /app/Takerman.Dating.Services/
RUN dotnet restore

WORKDIR /src
COPY Takerman.Dating.Data/. ./Takerman.Dating.Data/
COPY Takerman.Dating.Services/. ./Takerman.Dating.Services/
COPY ["takerman.dating.client/nuget.config", "takerman.dating.client/"]
COPY ["Takerman.Dating.Server/Takerman.Dating.Server.csproj", "Takerman.Dating.Server/"]
COPY ["takerman.dating.client/takerman.dating.client.esproj", "takerman.dating.client/"]
RUN dotnet restore "./Takerman.Dating.Server/./Takerman.Dating.Server.csproj"
COPY . .
WORKDIR "/src/Takerman.Dating.Server"
RUN dotnet build "./Takerman.Dating.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Takerman.Dating.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.Dating.Server.dll"]