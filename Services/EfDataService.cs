using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class EfDataService : IDataService
    {
        private readonly RepairOrderContext _context;

        public EfDataService(RepairOrderContext context)
        {
            _context = context;
        }

        // Customer operations
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<CustomerWithOrdersDto?> GetCustomerWithOrdersAsync(int customerId)
        {
            var customer = await _context.Customers
                .Include(c => c.RepairOrders)
                .ThenInclude(ro => ro.Vehicle)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null) return null;

            return new CustomerWithOrdersDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                RepairOrders = customer.RepairOrders.Select(ro => new RepairOrderSummaryDto
                {
                    Id = ro.Id,
                    CreatedDate = ro.CreatedDate,
                    Description = ro.Description,
                    EstimatedCost = ro.EstimatedCost,
                    Status = ro.Status,
                    VehicleInfo = $"{ro.Vehicle?.Year} {ro.Vehicle?.Make} {ro.Vehicle?.Model}"
                }).ToList()
            };
        }

        // Vehicle operations
        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<object?> GetVehicleWithHistoryAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.RepairOrders)
                .ThenInclude(ro => ro.Customer)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle == null) return null;

            return new
            {
                Id = vehicle.Id,
                Year = vehicle.Year,
                Make = vehicle.Make,
                Model = vehicle.Model,
                VehicleInfo = $"{vehicle.Year} {vehicle.Make} {vehicle.Model}",
                RepairHistory = vehicle.RepairOrders.Select(ro => new
                {
                    Id = ro.Id,
                    CreatedDate = ro.CreatedDate,
                    Description = ro.Description,
                    EstimatedCost = ro.EstimatedCost,
                    Status = ro.Status,
                    CustomerName = $"{ro.Customer?.FirstName} {ro.Customer?.LastName}"
                }).OrderByDescending(ro => ro.CreatedDate).ToList(),
                TotalRepairs = vehicle.RepairOrders.Count,
                TotalCost = vehicle.RepairOrders.Sum(ro => ro.EstimatedCost)
            };
        }

        // Repair order operations
        public async Task<List<RepairOrder>> GetAllRepairOrdersAsync()
        {
            return await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .ToListAsync();
        }

        public async Task<RepairOrder?> GetRepairOrderByIdAsync(int id)
        {
            return await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .FirstOrDefaultAsync(ro => ro.Id == id);
        }

        public async Task<RepairOrder> AddRepairOrderAsync(RepairOrder repairOrder)
        {
            repairOrder.CreatedDate = DateTime.Now;
            _context.RepairOrders.Add(repairOrder);
            await _context.SaveChangesAsync();

            // Load navigation properties
            await _context.Entry(repairOrder)
                .Reference(ro => ro.Customer)
                .LoadAsync();
            await _context.Entry(repairOrder)
                .Reference(ro => ro.Vehicle)
                .LoadAsync();

            return repairOrder;
        }

        public async Task<List<RepairOrder>> SearchRepairOrdersByCustomerLastNameAsync(string lastName)
        {
            return await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .Where(ro => ro.Customer!.LastName.Contains(lastName))
                .ToListAsync();
        }

        public async Task<List<RepairOrder>> GetRepairOrdersByStatusAsync(string status)
        {
            return await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .Where(ro => ro.Status == status)
                .ToListAsync();
        }

        public async Task<RepairOrder?> UpdateRepairOrderStatusAsync(int id, string status)
        {
            var order = await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .FirstOrDefaultAsync(ro => ro.Id == id);

            if (order == null) return null;

            order.Status = status;
            await _context.SaveChangesAsync();
            
            return order;
        }

        public async Task<object> GetSummaryStatisticsAsync()
        {
            var totalCustomers = await _context.Customers.CountAsync();
            var totalVehicles = await _context.Vehicles.CountAsync();
            var totalOrders = await _context.RepairOrders.CountAsync();
            
            var completedOrders = await _context.RepairOrders
                .Where(ro => ro.Status == "Completed")
                .ToListAsync();
            
            var pendingOrders = await _context.RepairOrders
                .Where(ro => ro.Status != "Completed" && ro.Status != "Cancelled")
                .ToListAsync();

            var totalRevenue = completedOrders.Sum(ro => ro.EstimatedCost);
            var pendingRevenue = pendingOrders.Sum(ro => ro.EstimatedCost);

            var statusBreakdown = await _context.RepairOrders
                .GroupBy(ro => ro.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var topCustomers = await _context.RepairOrders
                .Include(ro => ro.Customer)
                .GroupBy(ro => ro.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    CustomerName = g.First().Customer!.FirstName + " " + g.First().Customer.LastName,
                    OrderCount = g.Count(),
                    TotalSpent = g.Sum(ro => ro.EstimatedCost)
                })
                .OrderByDescending(c => c.TotalSpent)
                .Take(5)
                .ToListAsync();

            var allOrders = await _context.RepairOrders.ToListAsync();
            var averageOrderValue = allOrders.Any() ? allOrders.Average(ro => ro.EstimatedCost) : 0;

            return new
            {
                TotalCustomers = totalCustomers,
                TotalVehicles = totalVehicles,
                TotalRepairOrders = totalOrders,
                CompletedRevenue = totalRevenue,
                PendingRevenue = pendingRevenue,
                StatusBreakdown = statusBreakdown,
                TopCustomers = topCustomers,
                AverageOrderValue = averageOrderValue
            };
        }
    }
}