# Temel bir imaj seçiyoruz
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Uygulama için gerekli olan build imajını belirliyoruz
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Bookory klasörünü kopyalıyoruz
COPY ["Bookory/Bookory.API/Bookory.API.csproj", "Bookory.API/"]
COPY ["Bookory/Bookory.Business/Bookory.Business.csproj", "Bookory.Business/"]
COPY ["Bookory/Bookory.Core/Bookory.Core.csproj", "Bookory.Core/"]
COPY ["Bookory/Bookory.DataAccess/Bookory.DataAccess.csproj", "Bookory.DataAccess/"]

# NuGet paketlerini indiriyoruz
RUN dotnet restore "Bookory.API/Bookory.API.csproj"

# Projeyi build ediyoruz
COPY . .
WORKDIR "/src/Bookory/Bookory.API"
RUN dotnet build "Bookory.API.csproj" -c Release -o /app/build

# Uygulamanın publish edilmesi
FROM build AS publish
RUN dotnet publish "Bookory.API.csproj" -c Release -o /app/publish

# Uygulamanın son hali
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookory.API.dll"]
