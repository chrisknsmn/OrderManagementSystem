namespace OrderManagementSystem.Models
{
    public class CustomerWithOrdersDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<RepairOrderSummaryDto> RepairOrders { get; set; } = new List<RepairOrderSummaryDto>();
    }
}