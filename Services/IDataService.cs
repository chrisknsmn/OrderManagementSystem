using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public interface IDataService
    {
        // Customer operations
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(int id);

        // Vehicle operations
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(int id);
        Task<Vehicle> AddVehicleAsync(Vehicle vehicle);
        Task<bool> DeleteVehicleAsync(int id);

        // Repair order operations
        Task<List<RepairOrder>> GetAllRepairOrdersAsync();
        Task<RepairOrder?> GetRepairOrderByIdAsync(int id);
        Task<RepairOrder> AddRepairOrderAsync(RepairOrder repairOrder);
        Task<List<RepairOrder>> GetRepairOrdersByStatusAsync(string status);
        Task<bool> DeleteRepairOrderAsync(int id);
    }
}