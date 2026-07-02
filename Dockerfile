# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore dependencies trước (tận dụng Docker layer cache)
COPY ["FirstAidAPI.csproj", "."]
RUN dotnet restore "FirstAidAPI.csproj"

# Copy toàn bộ source code
COPY . .

# Build và publish
RUN dotnet publish "FirstAidAPI.csproj" -c Release -o /app/publish

# Stage 2: Runtime (image nhỏ hơn, không có SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Render dùng PORT env variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

EXPOSE 8080

ENTRYPOINT ["dotnet", "FirstAidAPI.dll"]
