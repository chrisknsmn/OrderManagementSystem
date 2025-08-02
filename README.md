# Repair Order Management System

A simple repair order management system with a Console App frontend and Web API backend.

## Project Structure

```
OrderManagementSystem/
├── Controllers/                     # API Controllers
│   ├── CustomersController.cs      # Customer management endpoints
│   ├── VehiclesController.cs       # Vehicle management endpoints  
│   └── RepairOrdersController.cs   # Repair order management endpoints
├── Models/                          # API Data models
│   ├── Customer.cs                 # Customer entity
│   ├── Vehicle.cs                  # Vehicle entity
│   └── RepairOrder.cs              # RepairOrder entity
├── Services/                        # Business logic
│   └── DataService.cs              # In-memory data service
├── RepairOrderSystem.Console/       # Console application
│   ├── Models/                     # Console app models
│   ├── Services/                   # API communication services
│   └── Program.cs                  # Main console interface
└── README.md                       # This file
```

## Features

### Backend API (Web API)
- **Customer Management**: Add and retrieve customers
- **Vehicle Management**: Add and retrieve vehicles  
- **Repair Order Management**: Create repair orders and search by customer name
- **In-memory Storage**: All data stored in memory with sample data pre-loaded
- **CORS Enabled**: Configured for local development

### Frontend (Console App)
- **Interactive Menu**: User-friendly console interface
- **Add Customers**: Input customer details (name, phone)
- **Add Vehicles**: Input vehicle details (year, make, model)
- **Create Repair Orders**: Associate customers with vehicles
- **Search Functionality**: Find repair orders by customer last name
- **View All Data**: Display all customers, vehicles, and repair orders

## How to Run

### Step 1: Start the API
1. Open a terminal/command prompt
2. Navigate to the project directory:
   ```bash
   cd C:\Users\chris\source\repos\OrderManagementSystem
   ```
3. Run the API:
   ```bash
   dotnet run
   ```
4. The API will start at `http://localhost:5027`
5. You can view the Swagger documentation at `http://localhost:5027/swagger`

### Step 2: Run the Console App
1. Open a **second** terminal/command prompt
2. Navigate to the console app directory:
   ```bash
   cd C:\Users\chris\source\repos\OrderManagementSystem\RepairOrderSystem.Console
   ```
3. Run the console application:
   ```bash
   dotnet run
   ```

## Usage Instructions

1. **Ensure the API is running first** - the console app will display an error if it can't connect
2. Use the numbered menu options to navigate
3. Follow the prompts to enter data
4. Use option 5 to view all current data
5. Use option 6 to exit

### Sample Data
The system comes pre-loaded with sample data:
- 3 customers (John Smith, Jane Johnson, Bob Wilson)
- 3 vehicles (2020 Toyota Camry, 2019 Honda Civic, 2021 Ford F-150)
- 2 repair orders (existing associations)

## API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Add new customer

### Vehicles  
- `GET /api/vehicles` - Get all vehicles
- `GET /api/vehicles/{id}` - Get vehicle by ID
- `POST /api/vehicles` - Add new vehicle

### Repair Orders
- `GET /api/repairorders` - Get all repair orders
- `GET /api/repairorders/{id}` - Get repair order by ID
- `GET /api/repairorders/search?lastName={name}` - Search by customer last name
- `POST /api/repairorders` - Create new repair order

## Technical Details

- **Backend**: ASP.NET Core Web API (.NET 8)
- **Frontend**: Console Application (.NET 8)
- **Data Storage**: In-memory collections (no database required)
- **Communication**: HTTP/JSON between console app and API
- **CORS**: Enabled for local development
- **Error Handling**: Basic validation and error responses

## Architecture Patterns

- **Separation of Concerns**: Models, Services, Controllers clearly separated
- **Service Layer**: DataService handles all data operations
- **Repository Pattern**: DataService acts as repository for in-memory data
- **RESTful API**: Standard HTTP methods and status codes
- **Dependency Injection**: DataService registered as singleton

## Build and Development

To build the entire solution:
```bash
dotnet build
```

To clean the solution:
```bash
dotnet clean
```

## Submission Notes

This project demonstrates:
- Clean code architecture with separation of concerns
- RESTful API design principles
- User-friendly console interface
- Proper error handling and validation
- In-memory data management
- Cross-project communication via HTTP