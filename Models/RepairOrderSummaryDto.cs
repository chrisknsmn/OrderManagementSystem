namespace OrderManagementSystem.Models
{
    public class RepairOrderSummaryDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public string Status { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty; // e.g., "2020 Toyota Camry"
    }
}