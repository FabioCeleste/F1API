FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["F1RestAPI/F1RestAPI.csproj", "F1RestAPI/"]
COPY ["F1RestAPI.sln", "."]

RUN dotnet restore "F1RestAPI/F1RestAPI.csproj"

COPY . .

WORKDIR "/src/F1RestAPI"
RUN dotnet build "F1RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "F1RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "F1RestAPI.dll"]
