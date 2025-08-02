namespace RepairOrderSystem.Console.Models
{
    public class RepairOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Status { get; set; } = "Open";
        
        // Navigation properties - Many repair orders belong to one customer/vehicle
        public Customer? Customer { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}