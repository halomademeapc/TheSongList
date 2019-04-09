# TheSongList [![Build Status](https://travis-ci.org/halomademeapc/TheSongList.png?branch=master)](https://travis-ci.org/halomademeapc/TheSongList)
For all the weird people who track songs in TV shows.

## Quickstart
```powershell
docker run --name thesonglist -d -p 80:80 halomademeapc/thesonglist
```
You will need to provide your own appsettings.json

## Configuration
Additional configuration is needed for authentication
```json
{
    "Authentication": {
        "Google": {
            "ClientId": "YOUR_ID",
            "ClientSecret": "YOUR_SECRET"
        }
    },
    "Lastfm": {
        "ClientId": "YOUR_ID",
        "ClientSecret": "YOUR_SECRET"
    }
}
```

## Requirements
**To build**
* .NET Core 2.2 SDK
* Node.js

**To run**
* .NET Core 2.2 Runtime

## Building
```powershell
dotnet publish ./PokeR.csproj
```

## Running
```powershell
dotnet ./PokeR.csproj
```