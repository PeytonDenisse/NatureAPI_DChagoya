FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App

# Copiar todo el código
COPY . ./

# (opcional) solo para debug
RUN ls

# Restaurar paquetes del proyecto correcto
RUN dotnet restore ./NatureAPI.csproj

# (opcional) ver qué hay en /App
RUN ls /App

# Publicar en Release
RUN dotnet publish ./NatureAPI.csproj -c Release -o /App/build


# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
RUN apt-get update -qq && apt-get -y install libgdiplus libc6-dev 
RUN apt-get update && apt-get install -y wget fontconfig

RUN mkdir -p /usr/share/fonts/truetype/poppins && \
    wget -O /usr/share/fonts/truetype/poppins/Poppins-Regular.ttf https://github.com/google/fonts/raw/main/ofl/poppins/Poppins-Regular.ttf && \
    wget -O /usr/share/fonts/truetype/poppins/Poppins-Bold.ttf https://github.com/google/fonts/raw/main/ofl/poppins/Poppins-Bold.ttf && \
    fc-cache -f -v

WORKDIR /App
COPY --from=build-env /App/build .
COPY ./Rotativa ./Rotativa
COPY ./Templates ./Templates
RUN chmod 755 /App/Rotativa/Linux/wkhtmltopdf
ENTRYPOINT ["dotnet", "NatureAPI.dll"]
