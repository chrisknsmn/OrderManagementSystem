using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Make { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;
        
        // Navigation property - One vehicle can have many repair orders
        public virtual ICollection<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
    }
}