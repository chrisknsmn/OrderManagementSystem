using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementSystem.Models
{
    public class RepairOrder
    {
        public int Id { get; set; }
        
        [Required]
        public int CustomerId { get; set; }
        
        [Required]
        public int VehicleId { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal EstimatedCost { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Open";
        
        // Navigation properties - Many repair orders belong to one customer/vehicle
        public virtual Customer? Customer { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
    }
}