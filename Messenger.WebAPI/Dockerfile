FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Messenger.WebAPI/Messenger.WebAPI.csproj", "Messenger.WebAPI/"]
RUN dotnet restore "Messenger.WebAPI/Messenger.WebAPI.csproj"
COPY . .
WORKDIR "/src/Messenger.WebAPI"
RUN dotnet build "Messenger.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.WebAPI.dll"]
