# syntax=docker/dockerfile:1
# Bygges fra repo-roden: docker build -f Consumer/Dockerfile -t consumer .

# ---------- Stage 1: byg ----------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Kun projektfilerne først -> restore-laget genbruges, så længe .csproj er uændret
COPY Shared/Shared.csproj Shared/
COPY Consumer/Consumer.csproj Consumer/
RUN dotnet restore Consumer/Consumer.csproj

# Derefter resten af koden
COPY Shared/ Shared/
COPY Consumer/ Consumer/
RUN dotnet publish Consumer/Consumer.csproj -c Release -o /app --no-restore

# ---------- Stage 2: kør ----------
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS final
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "Consumer.dll"]
