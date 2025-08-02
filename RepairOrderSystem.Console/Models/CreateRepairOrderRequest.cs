namespace RepairOrderSystem.Console.Models
{
    public class CreateRepairOrderRequest
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
    }
}