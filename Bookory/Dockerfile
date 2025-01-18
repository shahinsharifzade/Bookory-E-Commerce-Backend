# Base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy solution and restore dependencies
COPY ["Bookory.sln", "./"]
COPY ["src/Bookory.API/Bookory.API.csproj", "src/Bookory.API/"]
COPY ["src/Bookory.Business/Bookory.Business.csproj", "src/Bookory.Business/"]
COPY ["src/Bookory.Core/Bookory.Core.csproj", "src/Bookory.Core/"]
COPY ["src/Bookory.DataAccess/Bookory.DataAccess.csproj", "src/Bookory.DataAccess/"]
RUN dotnet restore "Bookory.sln"

# Copy entire source code and build
COPY . .
WORKDIR "/src/src/Bookory.API"
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bookory.API.dll"]
