# Temel bir imaj seçiyoruz
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Uygulama için gerekli olan build imajını belirliyoruz
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# .csproj dosyalarını kopyalıyoruz - Yolları düzelttik
COPY ["Bookory.API/Bookory.API.csproj", "Bookory.API/"]
COPY ["Bookory.Business/Bookory.Business.csproj", "Bookory.Business/"]
COPY ["Bookory.Core/Bookory.Core.csproj", "Bookory.Core/"]
COPY ["Bookory.DataAccess/Bookory.DataAccess.csproj", "Bookory.DataAccess/"]

# NuGet paketlerini indiriyoruz
RUN dotnet restore "Bookory.API/Bookory.API.csproj"

# Tüm kaynak kodunu kopyalıyoruz
COPY . .
WORKDIR "/src/Bookory.API"
RUN dotnet build "Bookory.API.csproj" -c Release -o /app/build

# Uygulamanın publish edilmesi
FROM build AS publish
RUN dotnet publish "Bookory.API.csproj" -c Release -o /app/publish

# Uygulamanın son hali
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookory.API.dll"]