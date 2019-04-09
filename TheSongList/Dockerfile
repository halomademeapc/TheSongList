FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["TheSongList/TheSongList.csproj", "TheSongList/"]
RUN dotnet restore "TheSongList/TheSongList.csproj"
COPY . .
WORKDIR "/src/TheSongList"
RUN dotnet build "TheSongList.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TheSongList.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TheSongList.dll"]