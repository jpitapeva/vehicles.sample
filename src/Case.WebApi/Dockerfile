FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
USER root
RUN apt-get update -y
RUN apt-get install -y gss-ntlmssp
WORKDIR /src

# Copy everything
COPY src src
COPY shared-settings shared-settings
COPY NuGet.Config /root/.nuget/NuGet/
COPY NuGet.Config /opt/app-root/.nuget/NuGet/
ENV DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
WORKDIR /src/src/Case.WebApi

# Build and publish a release
RUN dotnet restore 
RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app
RUN ln -sf /usr/share/zoneinfo/America/Sao_Paulo /etc/localtime

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
RUN useradd -ms /bin/bash default
USER default
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Case.WebApi.dll"]