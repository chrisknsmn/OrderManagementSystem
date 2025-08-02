using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class DataService
    {
        private readonly List<Customer> _customers = new();
        private readonly List<Vehicle> _vehicles = new();
        private readonly List<RepairOrder> _repairOrders = new();
        
        private int _nextCustomerId = 1;
        private int _nextVehicleId = 1;
        private int _nextRepairOrderId = 1;

        public DataService()
        {
            SeedData();
        }

        private void SeedData()
        {
            // Seed customers
            _customers.AddRange(new[]
            {
                new Customer { Id = _nextCustomerId++, FirstName = "John", LastName = "Smith", PhoneNumber = "555-0123" },
                new Customer { Id = _nextCustomerId++, FirstName = "Jane", LastName = "Johnson", PhoneNumber = "555-0456" },
                new Customer { Id = _nextCustomerId++, FirstName = "Bob", LastName = "Wilson", PhoneNumber = "555-0789" },
                new Customer { Id = _nextCustomerId++, FirstName = "Alice", LastName = "Brown", PhoneNumber = "555-0321" },
                new Customer { Id = _nextCustomerId++, FirstName = "Mike", LastName = "Davis", PhoneNumber = "555-0654" }
            });

            // Seed vehicles
            _vehicles.AddRange(new[]
            {
                new Vehicle { Id = _nextVehicleId++, Year = 2020, Make = "Toyota", Model = "Camry" },
                new Vehicle { Id = _nextVehicleId++, Year = 2019, Make = "Honda", Model = "Civic" },
                new Vehicle { Id = _nextVehicleId++, Year = 2021, Make = "Ford", Model = "F-150" },
                new Vehicle { Id = _nextVehicleId++, Year = 2018, Make = "Chevrolet", Model = "Malibu" },
                new Vehicle { Id = _nextVehicleId++, Year = 2022, Make = "BMW", Model = "X3" },
                new Vehicle { Id = _nextVehicleId++, Year = 2020, Make = "Audi", Model = "A4" }
            });

            // Seed repair orders with enhanced data
            _repairOrders.AddRange(new[]
            {
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-15), Description = "Oil change and brake inspection", EstimatedCost = 150.00m, Status = "Completed" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-10), Description = "Transmission repair", EstimatedCost = 2500.00m, Status = "In Progress" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-5), Description = "Replace air filter and spark plugs", EstimatedCost = 200.00m, Status = "Open" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 3, VehicleId = 3, CreatedDate = DateTime.Now.AddDays(-3), Description = "Tire rotation and alignment", EstimatedCost = 120.00m, Status = "Completed" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 4, VehicleId = 4, CreatedDate = DateTime.Now.AddDays(-7), Description = "Engine diagnostic and repair", EstimatedCost = 800.00m, Status = "In Progress" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-2), Description = "Battery replacement", EstimatedCost = 180.00m, Status = "Open" },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 5, VehicleId = 5, CreatedDate = DateTime.Now.AddDays(-1), Description = "Annual maintenance service", EstimatedCost = 450.00m, Status = "Open" }
            });

            // Load relationships
            LoadRelationships();
        }

        private void LoadRelationships()
        {
            foreach (var customer in _customers)
            {
                customer.RepairOrders = _repairOrders.Where(ro => ro.CustomerId == customer.Id).ToList();
            }

            foreach (var vehicle in _vehicles)
            {
                vehicle.RepairOrders = _repairOrders.Where(ro => ro.VehicleId == vehicle.Id).ToList();
            }

            foreach (var repairOrder in _repairOrders)
            {
                repairOrder.Customer = _customers.FirstOrDefault(c => c.Id == repairOrder.CustomerId);
                repairOrder.Vehicle = _vehicles.FirstOrDefault(v => v.Id == repairOrder.VehicleId);
            }
        }

        // Customer operations
        public List<Customer> GetAllCustomers() => _customers.ToList();
        
        public Customer? GetCustomerById(int id) => _customers.FirstOrDefault(c => c.Id == id);
        
        public Customer AddCustomer(Customer customer)
        {
            customer.Id = _nextCustomerId++;
            _customers.Add(customer);
            return customer;
        }

        public CustomerWithOrdersDto? GetCustomerWithOrders(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null) return null;

            var customerOrders = _repairOrders.Where(ro => ro.CustomerId == customerId).ToList();
            
            return new CustomerWithOrdersDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                RepairOrders = customerOrders.Select(ro => new RepairOrderSummaryDto
                {
                    Id = ro.Id,
                    CreatedDate = ro.CreatedDate,
                    Description = ro.Description,
                    EstimatedCost = ro.EstimatedCost,
                    Status = ro.Status,
                    VehicleInfo = GetVehicleDisplayString(ro.VehicleId)
                }).ToList()
            };
        }

        // Vehicle operations
        public List<Vehicle> GetAllVehicles() => _vehicles.ToList();
        
        public Vehicle? GetVehicleById(int id) => _vehicles.FirstOrDefault(v => v.Id == id);
        
        public Vehicle AddVehicle(Vehicle vehicle)
        {
            vehicle.Id = _nextVehicleId++;
            _vehicles.Add(vehicle);
            return vehicle;
        }

        public object? GetVehicleWithHistory(int vehicleId)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == vehicleId);
            if (vehicle == null) return null;

            var vehicleOrders = _repairOrders.Where(ro => ro.VehicleId == vehicleId).ToList();
            
            return new
            {
                Id = vehicle.Id,
                Year = vehicle.Year,
                Make = vehicle.Make,
                Model = vehicle.Model,
                VehicleInfo = $"{vehicle.Year} {vehicle.Make} {vehicle.Model}",
                RepairHistory = vehicleOrders.Select(ro => new
                {
                    Id = ro.Id,
                    CreatedDate = ro.CreatedDate,
                    Description = ro.Description,
                    EstimatedCost = ro.EstimatedCost,
                    Status = ro.Status,
                    CustomerName = GetCustomerDisplayString(ro.CustomerId)
                }).OrderByDescending(ro => ro.CreatedDate).ToList(),
                TotalRepairs = vehicleOrders.Count,
                TotalCost = vehicleOrders.Sum(ro => ro.EstimatedCost)
            };
        }

        // Repair order operations
        public List<RepairOrder> GetAllRepairOrders()
        {
            var orders = _repairOrders.ToList();
            foreach (var order in orders)
            {
                order.Customer = GetCustomerById(order.CustomerId);
                order.Vehicle = GetVehicleById(order.VehicleId);
            }
            return orders;
        }
        
        public RepairOrder? GetRepairOrderById(int id)
        {
            var order = _repairOrders.FirstOrDefault(r => r.Id == id);
            if (order != null)
            {
                order.Customer = GetCustomerById(order.CustomerId);
                order.Vehicle = GetVehicleById(order.VehicleId);
            }
            return order;
        }
        
        public RepairOrder AddRepairOrder(RepairOrder repairOrder)
        {
            repairOrder.Id = _nextRepairOrderId++;
            repairOrder.CreatedDate = DateTime.Now;
            _repairOrders.Add(repairOrder);
            
            repairOrder.Customer = GetCustomerById(repairOrder.CustomerId);
            repairOrder.Vehicle = GetVehicleById(repairOrder.VehicleId);
            
            // Update navigation properties
            LoadRelationships();
            
            return repairOrder;
        }
        
        public List<RepairOrder> SearchRepairOrdersByCustomerLastName(string lastName)
        {
            var orders = GetAllRepairOrders();
            return orders.Where(o => o.Customer?.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }

        public List<RepairOrder> GetRepairOrdersByStatus(string status)
        {
            var orders = GetAllRepairOrders();
            return orders.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public RepairOrder? UpdateRepairOrderStatus(int id, string status)
        {
            var order = _repairOrders.FirstOrDefault(ro => ro.Id == id);
            if (order == null) return null;

            order.Status = status;
            
            // Load navigation properties
            order.Customer = GetCustomerById(order.CustomerId);
            order.Vehicle = GetVehicleById(order.VehicleId);
            
            return order;
        }

        public object GetSummaryStatistics()
        {
            var totalCustomers = _customers.Count;
            var totalVehicles = _vehicles.Count;
            var totalOrders = _repairOrders.Count;
            var totalRevenue = _repairOrders.Where(ro => ro.Status == "Completed").Sum(ro => ro.EstimatedCost);
            var pendingRevenue = _repairOrders.Where(ro => ro.Status != "Completed" && ro.Status != "Cancelled").Sum(ro => ro.EstimatedCost);
            
            var statusBreakdown = _repairOrders.GroupBy(ro => ro.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            var topCustomers = _repairOrders.GroupBy(ro => ro.CustomerId)
                .Select(g => new 
                { 
                    CustomerId = g.Key,
                    CustomerName = GetCustomerDisplayString(g.Key),
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(ro => ro.EstimatedCost)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToList();

            return new
            {
                TotalCustomers = totalCustomers,
                TotalVehicles = totalVehicles,
                TotalRepairOrders = totalOrders,
                CompletedRevenue = totalRevenue,
                PendingRevenue = pendingRevenue,
                StatusBreakdown = statusBreakdown,
                TopCustomers = topCustomers,
                AverageOrderValue = totalOrders > 0 ? _repairOrders.Average(ro => ro.EstimatedCost) : 0
            };
        }

        // Helper methods
        private string GetCustomerDisplayString(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == customerId);
            return customer != null ? $"{customer.FirstName} {customer.LastName}" : "Unknown Customer";
        }

        private string GetVehicleDisplayString(int vehicleId)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == vehicleId);
            return vehicle != null ? $"{vehicle.Year} {vehicle.Make} {vehicle.Model}" : "Unknown Vehicle";
        }
    }
}