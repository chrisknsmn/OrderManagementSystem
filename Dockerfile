# Build stage for frontend
FROM node:18-alpine as frontend-build
WORKDIR /app/frontend
COPY frontend/package*.json ./
RUN npm install
COPY frontend/ ./
RUN npm run build

# Build stage for backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=backend-build /app/out .
COPY --from=frontend-build /app/frontend/dist ./wwwroot

# Create directory for SQLite database
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "OrderManagementSystem.dll"]