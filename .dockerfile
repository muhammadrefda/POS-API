# Resep Backend (Dockerfile)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy & Restore
COPY *.csproj ./
RUN dotnet restore

# Copy segalanya & Build
COPY . ./
RUN dotnet publish -c Release -o out

# Sajikan!
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "POS-API.dll"]