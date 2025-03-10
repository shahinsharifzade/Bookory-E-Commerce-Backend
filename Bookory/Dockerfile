# Base image (runtime için .NET 6.0 kullanıyoruz)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image (.NET 6.0 SDK kullanıyoruz)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Solution ve project dosyalarını kopyalıyoruz
COPY ["Bookory.sln", "./"]
COPY ["src/Bookory.API/Bookory.API.csproj", "./src/Bookory.API/"]
COPY ["src/Bookory.Business/Bookory.Business.csproj", "./src/Bookory.Business/"]
COPY ["src/Bookory.Core/Bookory.Core.csproj", "./src/Bookory.Core/"]
COPY ["src/Bookory.DataAccess/Bookory.DataAccess.csproj", "./src/Bookory.DataAccess/"]
RUN dotnet restore "Bookory.sln"

# Kaynak kodları kopyala ve build et
COPY . .
WORKDIR "/src/src/Bookory.API"
RUN dotnet publish -c Release -o /app/publish

# Çalışma için runtime image'ı kullanıyoruz
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Bookory.API.dll"]
