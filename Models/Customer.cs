using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
        
        // Navigation property - One customer can have many repair orders
        public virtual ICollection<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
    }
}