# https://hub.docker.com/_/microsoft-dotnet-sdk/
# Debian 10 based
# FROM mcr.microsoft.com/dotnet/sdk:latest AS build
# Ubuntu 20.04
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

# install nodejs & npm
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs apt-utils
RUN node --version
RUN npm install -g npm
RUN npm --version


WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Directory.Build.props .

# project files for restore

# Base
COPY  Contracts.DAL.Base/*.csproj ./Contracts.DAL.Base/
COPY  Contracts.Domain.Base/*.csproj ./Contracts.Domain.Base/
COPY  DAL.Base/*.csproj ./DAL.Base/
COPY  DAL.Base.EF/*.csproj ./DAL.Base.EF/
COPY  Domain.Base/*.csproj ./Domain.Base/

# App
COPY  Contracts.DAL.App/*.csproj ./Contracts.DAL.App/
COPY  DAL.App.DTO/*.csproj ./DAL.App.DTO/
COPY  DAL.App.EF/*.csproj ./DAL.App.EF/
COPY  Domain.App/*.csproj ./Domain.App/
COPY  WebApp/*.csproj ./WebApp/


RUN dotnet restore


# copy everything else and build app
# Base
COPY  Contracts.DAL.Base/. ./Contracts.DAL.Base/
COPY  Contracts.Domain.Base/. ./Contracts.Domain.Base/
COPY  DAL.Base/. ./DAL.Base/
COPY  DAL.Base.EF/. ./DAL.Base.EF/
COPY  Domain.Base/. ./Domain.Base/

# App
COPY  Contracts.DAL.App/. ./Contracts.DAL.App/
COPY  DAL.App.DTO/. ./DAL.App.DTO/
COPY  DAL.App.EF/. ./DAL.App.EF/
COPY  Domain.App/. ./Domain.App/
COPY  WebApp/. ./WebApp/


# build and publish WebApp
WORKDIR /source/WebApp
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal


# set timezone
# https://serverfault.com/questions/683605/docker-container-time-timezone-will-not-reflect-changes
ENV TZ 'Europe/Tallinn'
RUN echo $TZ > /etc/timezone && \
    apt-get update && apt-get install -y tzdata && \
    rm /etc/localtime && \
    ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata && \
    apt-get clean \
WORKDIR /app
COPY --from=build /app ./
ENV ASPNETCORE_URLS http://*:$PORT
ENTRYPOINT ["dotnet", "WebApp.dll"]
