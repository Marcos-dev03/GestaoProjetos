FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["Gestão de projetos.csproj", "./"]

RUN dotnet restore "Gestão de projetos.csproj"

COPY . .

RUN dotnet publish "Gestão de projetos.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "dotnet 'Gestão de projetos.dll' --urls http://0.0.0.0:${PORT:-10000}"]