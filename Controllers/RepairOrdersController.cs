using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly DataService _dataService;

        public RepairOrdersController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public ActionResult<List<RepairOrder>> GetAll()
        {
            return Ok(_dataService.GetAllRepairOrders());
        }

        [HttpGet("{id}")]
        public ActionResult<RepairOrder> GetById(int id)
        {
            var repairOrder = _dataService.GetRepairOrderById(id);
            if (repairOrder == null)
                return NotFound();
            return Ok(repairOrder);
        }

        [HttpGet("search")]
        public ActionResult<List<RepairOrder>> SearchByCustomerLastName([FromQuery] string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                return BadRequest("Last name is required");

            var results = _dataService.SearchRepairOrdersByCustomerLastName(lastName);
            return Ok(results);
        }

        [HttpPost]
        public ActionResult<RepairOrder> Create([FromBody] RepairOrder repairOrder)
        {
            // Validate customer exists
            var customer = _dataService.GetCustomerById(repairOrder.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found");

            // Validate vehicle exists
            var vehicle = _dataService.GetVehicleById(repairOrder.VehicleId);
            if (vehicle == null)
                return BadRequest("Vehicle not found");

            var createdRepairOrder = _dataService.AddRepairOrder(repairOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdRepairOrder.Id }, createdRepairOrder);
        }

        // NEW: Get repair orders by status
        [HttpGet("status/{status}")]
        public ActionResult<List<RepairOrder>> GetRepairOrdersByStatus(string status)
        {
            var validStatuses = new[] { "Open", "In Progress", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}");
            }

            var orders = _dataService.GetRepairOrdersByStatus(status);
            return Ok(orders);
        }

        // NEW: Update repair order status
        [HttpPatch("{id}/status")]
        public ActionResult<RepairOrder> UpdateRepairOrderStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest("Status is required.");
            }

            var validStatuses = new[] { "Open", "In Progress", "Completed", "Cancelled" };
            if (!validStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}");
            }

            var updatedOrder = _dataService.UpdateRepairOrderStatus(id, request.Status);
            if (updatedOrder == null)
            {
                return NotFound($"Repair order with ID {id} not found.");
            }

            return Ok(updatedOrder);
        }
    }
}