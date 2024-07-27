FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Production
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
RUN --mount=type=secret,id=NUGET_PASSWORD NUGET_PASSWORD=$(cat /run/secrets/NUGET_PASSWORD) && echo NUGET_PASSWORD

WORKDIR /src

COPY Takerman.Dating.Data/. ./Takerman.Dating.Data/
COPY Takerman.Dating.Services/. ./Takerman.Dating.Services/
COPY Takerman.Dating.Models/. ./Takerman.Dating.Models/
COPY Takerman.Dating.Tests/. ./Takerman.Dating.Tests/

COPY ["takerman.dating.client/nuget.config", "./"]
COPY ["takerman.dating.client/nuget.config", "takerman.dating.client/"]

RUN sed -i "s|</configuration>|<packageSourceCredentials><github><add key=\"Username\" value=\"takerman\"/><add key=\"ClearTextPassword\" value=\"${NUGET_PASSWORD}\"/></github></packageSourceCredentials></configuration>|" nuget.config
RUN dotnet nuget add source https://nuget.pkg.github.com/takermanltd/index.json --name github
RUN dotnet nuget list source

COPY ["Takerman.Dating.Server/Takerman.Dating.Server.csproj", "Takerman.Dating.Server/"]
COPY ["takerman.dating.client/takerman.dating.client.esproj", "takerman.dating.client/"]
COPY ["takerman.dating.client/package.json", "package.json"]
COPY . .

RUN echo //npm.pkg.github.com/:_authToken=$NUGET_PASSWORD >> ~/.npmrc
RUN echo @takermanltd:registry=https://npm.pkg.github.com/ >> ~/.npmrc
RUN echo "user.email=tivanov@takerman.net" > .npmrc
RUN echo "user.name=takerman" > .npmrc
RUN echo "user.username=takerman" > .npmrc
RUN npm install --production
# RUN npm ci

WORKDIR "/src/Takerman.Dating.Server"
RUN dotnet clean "./Takerman.Dating.Server.csproj"
RUN dotnet restore "./Takerman.Dating.Server.csproj"
RUN dotnet build "./Takerman.Dating.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet test "./Takerman.Dating.Server.csproj"
RUN rm -f .npmrc

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Takerman.Dating.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Takerman.Dating.Server.dll"]