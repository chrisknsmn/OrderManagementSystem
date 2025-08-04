# Repair Order Management System

A modern, full-stack repair order management system built with **Vue.js 3**, **TypeScript**, **Bootstrap 5**, **ASP.NET Core 8**, and **Entity Framework Core**.

## Architecture

- **Frontend**: Vue.js 3 + TypeScript + Bootstrap 5 + Vite
- **Backend**: ASP.NET Core 8 Web API + Entity Framework Core
- **Database**: SQLite (development) / SQL Server (production)
- **Deployment**: Docker + Docker Compose

## Features

### Backend API
- ✅ RESTful API with Swagger documentation
- ✅ Entity Framework Core with proper relationships
- ✅ CRUD operations for Customers, Vehicles, and Repair Orders
- ✅ Advanced querying (search, filter by status, customer orders)
- ✅ Dashboard with business analytics
- ✅ Async/await patterns throughout
- ✅ Data validation and error handling

### Frontend SPA
- ✅ Modern Vue.js 3 with Composition API
- ✅ TypeScript for type safety
- ✅ Bootstrap 5 for responsive UI
- ✅ Vue Router for SPA navigation
- ✅ Axios for API communication
- ✅ Real-time status updates
- ✅ Modal dialogs and form validation

## Quick Start

### Option 1: Docker (Recommended for Deployment)

```bash
# Clone the repository
git clone <your-repo-url>
cd OrderManagementSystem

# Build and run with Docker Compose
docker-compose up --build

# Access the application
open http://localhost:8080
```

### Option 2: Local Development

**Prerequisites**: .NET 8 SDK, Node.js 18+

```bash
# Build the application
./build.sh

# Run the API
dotnet run

# Access the application
open http://localhost:5027
```

## Deployment Options

### 1. **Azure App Service**
```bash
# Install Azure CLI
az login

# Deploy to Azure
az webapp up --runtime "DOTNET|8.0" --sku F1 --name your-app-name
```

## Project Structure

```
OrderManagementSystem/
├── frontend/                    # Vue.js 3 + TypeScript frontend
│   ├── src/
│   │   ├── components/         # Reusable Vue components
│   │   ├── views/             # Page components
│   │   ├── services/          # API service layer
│   │   └── types/             # TypeScript type definitions
│   ├── package.json
│   └── vite.config.ts
├── Controllers/                # ASP.NET Core API controllers
├── Models/                     # Entity models and DTOs
├── Services/                   # Business logic layer
├── Data/                       # Entity Framework DbContext
├── Migrations/                 # Database migrations
├── wwwroot/                    # Built frontend files (auto-generated)
├── Dockerfile                  # Multi-stage Docker build
├── docker-compose.yml          # Docker Compose configuration
└── build.sh                    # Build script
```

## 🛠️ Development

### Backend Development
```bash
# Run API only
dotnet run

# Run with hot reload
dotnet watch run

# Entity Framework commands
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Frontend Development
```bash
cd frontend

# Install dependencies
npm install

# Run dev server with API proxy
npm run dev

# Build for production
npm run build

# Type checking
npm run type-check
```

## Configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: `Development` or `Production`
- `ConnectionStrings__DefaultConnection`: Database connection string

### Database Options
- **Development**: SQLite (no setup required)
- **Production**: SQL Server (configure connection string)

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `POST /api/customers` - Create customer
- `GET /api/customers/{id}/orders` - Get customer with orders

### Vehicles
- `GET /api/vehicles` - Get all vehicles
- `POST /api/vehicles` - Create vehicle
- `GET /api/vehicles/{id}/history` - Get vehicle repair history

### Repair Orders
- `GET /api/repairorders` - Get all repair orders
- `POST /api/repairorders` - Create repair order
- `GET /api/repairorders/status/{status}` - Filter by status
- `PATCH /api/repairorders/{id}/status` - Update status

### Dashboard
- `GET /api/dashboard/statistics` - Get business analytics

## Technical Highlights

- **Modern Tech Stack**: Latest versions of Vue 3, .NET 8, Bootstrap 5
- **Type Safety**: Full TypeScript implementation
- **Responsive Design**: Mobile-first Bootstrap components
- **Professional Architecture**: Clean separation of concerns
- **Database Integration**: Entity Framework with proper relationships
- **Containerized**: Docker-ready for any deployment platform
- **Production Ready**: Error handling, validation, logging

## License

This project is licensed under the MIT License.

---

**Built using Vue.js, TypeScript, Bootstrap, ASP.NET Core, and Entity Framework Core**