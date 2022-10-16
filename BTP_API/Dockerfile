FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BTP_API/BTP_API.csproj", "BTP_API/"]
RUN dotnet restore "BTP_API/BTP_API.csproj"
COPY . .
WORKDIR "/src/BTP_API"
RUN dotnet build "BTP_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BTP_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BTP_API.dll