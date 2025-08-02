namespace OrderManagementSystem.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        
        // Navigation property - One vehicle can have many repair orders
        public List<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
    }
}