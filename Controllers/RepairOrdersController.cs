using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepairOrdersController : ControllerBase
    {
        private readonly IDataService _dataService;

        public RepairOrdersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RepairOrder>>> GetAll()
        {
            var orders = await _dataService.GetAllRepairOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RepairOrder>> GetById(int id)
        {
            var repairOrder = await _dataService.GetRepairOrderByIdAsync(id);
            if (repairOrder == null)
                return NotFound();
            return Ok(repairOrder);
        }


        [HttpPost]
        public async Task<ActionResult<RepairOrder>> Create([FromBody] RepairOrder repairOrder)
        {
            // Validate customer exists
            var customer = await _dataService.GetCustomerByIdAsync(repairOrder.CustomerId);
            if (customer == null)
                return BadRequest("Customer not found");

            // Validate vehicle exists
            var vehicle = await _dataService.GetVehicleByIdAsync(repairOrder.VehicleId);
            if (vehicle == null)
                return BadRequest("Vehicle not found");

            var createdRepairOrder = await _dataService.AddRepairOrderAsync(repairOrder);
            return CreatedAtAction(nameof(GetById), new { id = createdRepairOrder.Id }, createdRepairOrder);
        }

        // NEW: Get repair orders by status
        [HttpGet("status/{status}")]
        public async Task<ActionResult<List<RepairOrder>>> GetRepairOrdersByStatus(string status)
        {
            var validStatuses = new[] { "Open", "In Progress", "Completed", "Cancelled" };
            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest($"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}");
            }

            var orders = await _dataService.GetRepairOrdersByStatusAsync(status);
            return Ok(orders);
        }


        // DELETE: Delete repair order
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRepairOrder(int id)
        {
            var deleted = await _dataService.DeleteRepairOrderAsync(id);
            if (!deleted)
            {
                return NotFound($"Repair order with ID {id} not found.");
            }

            return NoContent();
        }
    }
}