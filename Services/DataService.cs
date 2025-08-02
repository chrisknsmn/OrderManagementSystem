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
                new Customer { Id = _nextCustomerId++, FirstName = "Bob", LastName = "Wilson", PhoneNumber = "555-0789" }
            });

            // Seed vehicles
            _vehicles.AddRange(new[]
            {
                new Vehicle { Id = _nextVehicleId++, Year = 2020, Make = "Toyota", Model = "Camry" },
                new Vehicle { Id = _nextVehicleId++, Year = 2019, Make = "Honda", Model = "Civic" },
                new Vehicle { Id = _nextVehicleId++, Year = 2021, Make = "Ford", Model = "F-150" }
            });

            // Seed repair orders
            _repairOrders.AddRange(new[]
            {
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 1, VehicleId = 1, CreatedDate = DateTime.Now.AddDays(-5) },
                new RepairOrder { Id = _nextRepairOrderId++, CustomerId = 2, VehicleId = 2, CreatedDate = DateTime.Now.AddDays(-3) }
            });
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

        // Vehicle operations
        public List<Vehicle> GetAllVehicles() => _vehicles.ToList();
        
        public Vehicle? GetVehicleById(int id) => _vehicles.FirstOrDefault(v => v.Id == id);
        
        public Vehicle AddVehicle(Vehicle vehicle)
        {
            vehicle.Id = _nextVehicleId++;
            _vehicles.Add(vehicle);
            return vehicle;
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
            
            return repairOrder;
        }
        
        public List<RepairOrder> SearchRepairOrdersByCustomerLastName(string lastName)
        {
            var orders = GetAllRepairOrders();
            return orders.Where(o => o.Customer?.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase) == true).ToList();
        }
    }
}