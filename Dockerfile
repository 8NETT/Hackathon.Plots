# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

ARG RUNTIME=linux-x64

# Copy project files
COPY src/API/API.csproj src/API/
COPY src/Application/Application.csproj src/Application/
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/

# Restore
RUN dotnet restore src/API/API.csproj \
    -r $RUNTIME

# Copy remaining source
COPY . .

RUN dotnet publish src/API/API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    -r $RUNTIME \
    /p:UseAppHost=false

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8082
EXPOSE 8082

ENTRYPOINT ["dotnet", "API.dll"]