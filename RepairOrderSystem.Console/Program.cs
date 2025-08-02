using RepairOrderSystem.Console.Models;
using RepairOrderSystem.Console.Services;

namespace RepairOrderSystem.Console
{
    class Program
    {
        private static ApiService _apiService = new();

        static async Task Main(string[] args)
        {
            System.Console.WriteLine("=== Repair Order Management System ===");
            System.Console.WriteLine("Please ensure the API is running at http://localhost:5027");
            System.Console.WriteLine();

            while (true)
            {
                try
                {
                    await ShowMainMenu();
                }
                catch (HttpRequestException ex)
                {
                    System.Console.WriteLine($"Error connecting to API: {ex.Message}");
                    System.Console.WriteLine("Please ensure the API is running and try again.");
                    System.Console.WriteLine();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"An error occurred: {ex.Message}");
                    System.Console.WriteLine();
                }
            }
        }

        static async Task ShowMainMenu()
        {
            System.Console.WriteLine("=== Main Menu ===");
            System.Console.WriteLine("1. Add Customer");
            System.Console.WriteLine("2. Add Vehicle");
            System.Console.WriteLine("3. Create Repair Order");
            System.Console.WriteLine("4. Search Repair Orders");
            System.Console.WriteLine("5. View All Data");
            System.Console.WriteLine("6. Exit");
            System.Console.Write("Select an option: ");

            var choice = System.Console.ReadLine();
            System.Console.WriteLine();

            switch (choice)
            {
                case "1":
                    await AddCustomer();
                    break;
                case "2":
                    await AddVehicle();
                    break;
                case "3":
                    await CreateRepairOrder();
                    break;
                case "4":
                    await SearchRepairOrders();
                    break;
                case "5":
                    await ViewAllData();
                    break;
                case "6":
                    System.Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    System.Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            System.Console.WriteLine();
        }

        static async Task AddCustomer()
        {
            System.Console.WriteLine("=== Add Customer ===");
            
            System.Console.Write("First Name: ");
            var firstName = System.Console.ReadLine();
            
            System.Console.Write("Last Name: ");
            var lastName = System.Console.ReadLine();
            
            System.Console.Write("Phone Number: ");
            var phoneNumber = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                System.Console.WriteLine("All fields are required.");
                return;
            }

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber
            };

            var result = await _apiService.AddCustomerAsync(customer);
            if (result != null)
            {
                System.Console.WriteLine($"Customer added successfully! ID: {result.Id}");
            }
            else
            {
                System.Console.WriteLine("Failed to add customer.");
            }
        }

        static async Task AddVehicle()
        {
            System.Console.WriteLine("=== Add Vehicle ===");
            
            System.Console.Write("Year: ");
            if (!int.TryParse(System.Console.ReadLine(), out int year))
            {
                System.Console.WriteLine("Invalid year.");
                return;
            }
            
            System.Console.Write("Make: ");
            var make = System.Console.ReadLine();
            
            System.Console.Write("Model: ");
            var model = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(make) || string.IsNullOrWhiteSpace(model))
            {
                System.Console.WriteLine("Make and Model are required.");
                return;
            }

            var vehicle = new Vehicle
            {
                Year = year,
                Make = make,
                Model = model
            };

            var result = await _apiService.AddVehicleAsync(vehicle);
            if (result != null)
            {
                System.Console.WriteLine($"Vehicle added successfully! ID: {result.Id}");
            }
            else
            {
                System.Console.WriteLine("Failed to add vehicle.");
            }
        }

        static async Task CreateRepairOrder()
        {
            System.Console.WriteLine("=== Create Repair Order ===");
            
            // Show available customers
            var customers = await _apiService.GetCustomersAsync();
            System.Console.WriteLine("Available Customers:");
            foreach (var customer in customers)
            {
                System.Console.WriteLine($"  {customer.Id}: {customer.FirstName} {customer.LastName}");
            }
            
            System.Console.Write("Customer ID: ");
            if (!int.TryParse(System.Console.ReadLine(), out int customerId))
            {
                System.Console.WriteLine("Invalid Customer ID.");
                return;
            }

            // Show available vehicles
            var vehicles = await _apiService.GetVehiclesAsync();
            System.Console.WriteLine("Available Vehicles:");
            foreach (var vehicle in vehicles)
            {
                System.Console.WriteLine($"  {vehicle.Id}: {vehicle.Year} {vehicle.Make} {vehicle.Model}");
            }
            
            System.Console.Write("Vehicle ID: ");
            if (!int.TryParse(System.Console.ReadLine(), out int vehicleId))
            {
                System.Console.WriteLine("Invalid Vehicle ID.");
                return;
            }

            System.Console.Write("Description: ");
            var description = System.Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                System.Console.WriteLine("Description is required.");
                return;
            }

            System.Console.Write("Estimated Cost: $");
            if (!decimal.TryParse(System.Console.ReadLine(), out decimal estimatedCost))
            {
                System.Console.WriteLine("Invalid cost amount.");
                return;
            }

            var request = new CreateRepairOrderRequest
            {
                CustomerId = customerId,
                VehicleId = vehicleId,
                Description = description,
                EstimatedCost = estimatedCost
            };

            var result = await _apiService.AddRepairOrderAsync(request);
            if (result != null)
            {
                System.Console.WriteLine($"Repair Order created successfully!");
                System.Console.WriteLine($"  ID: {result.Id}");
                System.Console.WriteLine($"  Description: {result.Description}");
                System.Console.WriteLine($"  Estimated Cost: ${result.EstimatedCost:F2}");
                System.Console.WriteLine($"  Status: {result.Status}");
            }
            else
            {
                System.Console.WriteLine("Failed to create repair order. Please check that the Customer and Vehicle IDs are valid.");
            }
        }

        static async Task SearchRepairOrders()
        {
            System.Console.WriteLine("=== Search Repair Orders ===");
            
            System.Console.Write("Customer Last Name: ");
            var lastName = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(lastName))
            {
                System.Console.WriteLine("Last name is required.");
                return;
            }

            var results = await _apiService.SearchRepairOrdersByLastNameAsync(lastName);
            
            if (results.Any())
            {
                System.Console.WriteLine($"Found {results.Count} repair order(s):");
                foreach (var order in results)
                {
                    System.Console.WriteLine($"  Order ID: {order.Id}");
                    System.Console.WriteLine($"  Customer: {order.Customer?.FirstName} {order.Customer?.LastName}");
                    System.Console.WriteLine($"  Vehicle: {order.Vehicle?.Year} {order.Vehicle?.Make} {order.Vehicle?.Model}");
                    System.Console.WriteLine($"  Description: {order.Description}");
                    System.Console.WriteLine($"  Cost: ${order.EstimatedCost:F2} | Status: {order.Status}");
                    System.Console.WriteLine($"  Created: {order.CreatedDate:yyyy-MM-dd HH:mm}");
                    System.Console.WriteLine();
                }
            }
            else
            {
                System.Console.WriteLine("No repair orders found for that customer.");
            }
        }

        static async Task ViewAllData()
        {
            System.Console.WriteLine("=== All Data ===");
            
            // Show all customers
            var customers = await _apiService.GetCustomersAsync();
            System.Console.WriteLine("Customers:");
            foreach (var customer in customers)
            {
                System.Console.WriteLine($"  {customer.Id}: {customer.FirstName} {customer.LastName} - {customer.PhoneNumber}");
            }
            System.Console.WriteLine();

            // Show all vehicles
            var vehicles = await _apiService.GetVehiclesAsync();
            System.Console.WriteLine("Vehicles:");
            foreach (var vehicle in vehicles)
            {
                System.Console.WriteLine($"  {vehicle.Id}: {vehicle.Year} {vehicle.Make} {vehicle.Model}");
            }
            System.Console.WriteLine();

            // Show all repair orders
            var repairOrders = await _apiService.GetRepairOrdersAsync();
            System.Console.WriteLine("Repair Orders:");
            foreach (var order in repairOrders)
            {
                System.Console.WriteLine($"  Order {order.Id}: {order.Customer?.FirstName} {order.Customer?.LastName} - {order.Vehicle?.Year} {order.Vehicle?.Make} {order.Vehicle?.Model}");
                System.Console.WriteLine($"    Description: {order.Description}");
                System.Console.WriteLine($"    Cost: ${order.EstimatedCost:F2} | Status: {order.Status} | Created: {order.CreatedDate:yyyy-MM-dd}");
            }
        }
    }
}
