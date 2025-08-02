using System.Text;
using System.Text.Json;
using RepairOrderSystem.Console.Models;

namespace RepairOrderSystem.Console.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _baseUrl = "http://localhost:5027/api"; // HTTP port for API
        }

        // Customer operations
        public async Task<List<Customer>> GetCustomersAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/customers");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Customer>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Customer>();
        }

        public async Task<Customer?> AddCustomerAsync(Customer customer)
        {
            var json = JsonSerializer.Serialize(customer);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/customers", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Customer>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        // Vehicle operations
        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/vehicles");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Vehicle>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Vehicle>();
        }

        public async Task<Vehicle?> AddVehicleAsync(Vehicle vehicle)
        {
            var json = JsonSerializer.Serialize(vehicle);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/vehicles", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Vehicle>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        // Repair order operations
        public async Task<List<RepairOrder>> GetRepairOrdersAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/repairorders");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RepairOrder>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RepairOrder>();
        }

        public async Task<RepairOrder?> AddRepairOrderAsync(RepairOrder repairOrder)
        {
            var json = JsonSerializer.Serialize(repairOrder);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{_baseUrl}/repairorders", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RepairOrder>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            return null;
        }

        public async Task<RepairOrder?> AddRepairOrderAsync(CreateRepairOrderRequest request)
        {
            var repairOrder = new RepairOrder
            {
                CustomerId = request.CustomerId,
                VehicleId = request.VehicleId,
                Description = request.Description,
                EstimatedCost = request.EstimatedCost
            };
            
            return await AddRepairOrderAsync(repairOrder);
        }

        public async Task<List<RepairOrder>> SearchRepairOrdersByLastNameAsync(string lastName)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/repairorders/search?lastName={Uri.EscapeDataString(lastName)}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<RepairOrder>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<RepairOrder>();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}