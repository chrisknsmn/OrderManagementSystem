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


        public async Task<List<RepairOrder>> GetRepairOrdersByStatusAsync(string status)
        {
            return await _context.RepairOrders
                .Include(ro => ro.Customer)
                .Include(ro => ro.Vehicle)
                .Where(ro => ro.Status == status)
                .ToListAsync();
        }


        public async Task<bool> DeleteRepairOrderAsync(int id)
        {
            var order = await _context.RepairOrders.FindAsync(id);
            if (order == null)
                return false;

            _context.RepairOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return false;

            // Delete all related repair orders first (cascade delete)
            var relatedOrders = await _context.RepairOrders.Where(ro => ro.CustomerId == id).ToListAsync();
            _context.RepairOrders.RemoveRange(relatedOrders);

            // Delete the customer
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVehicleAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return false;

            // Delete all related repair orders first (cascade delete)
            var relatedOrders = await _context.RepairOrders.Where(ro => ro.VehicleId == id).ToListAsync();
            _context.RepairOrders.RemoveRange(relatedOrders);

            // Delete the vehicle
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}