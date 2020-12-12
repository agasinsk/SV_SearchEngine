#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_10.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["SearchEngine.Web/SearchEngine.Web.csproj", "SearchEngine.Web/"]
COPY ["SearchEngine.Service/SearchEngine.Service.csproj", "SearchEngine.Service/"]
COPY ["SearchEngine.Model/SearchEngine.Model.csproj", "SearchEngine.Model/"]
RUN dotnet restore "SearchEngine.Web/SearchEngine.Web.csproj"
COPY . .
WORKDIR "/src/SearchEngine.Web"
RUN dotnet build "SearchEngine.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SearchEngine.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SearchEngine.Web.dll"]
# Use the following instead for Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet SearchEngine.Web.dll