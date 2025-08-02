namespace OrderManagementSystem.Models
{
    public class RepairOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime CreatedDate { get; set; }
        
        // Navigation properties for easier display
        public Customer? Customer { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}