# Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Çözüm dosyasını kopyala ve restore yap
COPY Bookory.sln ./
COPY src/Bookory.API/Bookory.API.csproj ./src/Bookory.API/
COPY src/Bookory.Business/Bookory.Business.csproj ./src/Bookory.Business/
COPY src/Bookory.Core/Bookory.Core.csproj ./src/Bookory.Core/
COPY src/Bookory.DataAccess/Bookory.DataAccess.csproj ./src/Bookory.DataAccess/
RUN dotnet restore

# Projenin geri kalanını kopyala ve build et
COPY . .
WORKDIR /app/src/Bookory.API
RUN dotnet publish -c Release -o out

# Runtime aşaması
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/src/Bookory.API/out .

# Uygulamayı başlat
ENTRYPOINT ["dotnet", "Bookory.API.dll"]
