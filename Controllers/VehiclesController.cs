using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IDataService _dataService;

        public VehiclesController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Vehicle>>> GetAll()
        {
            var vehicles = await _dataService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetById(int id)
        {
            var vehicle = await _dataService.GetVehicleByIdAsync(id);
            if (vehicle == null)
                return NotFound();
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> Create([FromBody] Vehicle vehicle)
        {
            if (vehicle.Year < 1900 || vehicle.Year > DateTime.Now.Year + 1 ||
                string.IsNullOrWhiteSpace(vehicle.Make) || 
                string.IsNullOrWhiteSpace(vehicle.Model))
            {
                return BadRequest("Invalid vehicle data");
            }

            var createdVehicle = await _dataService.AddVehicleAsync(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
        }

        // NEW: Get vehicle with repair history
        [HttpGet("{id}/history")]
        public async Task<ActionResult<object>> GetVehicleHistory(int id)
        {
            var vehicleWithHistory = await _dataService.GetVehicleWithHistoryAsync(id);
            if (vehicleWithHistory == null)
                return NotFound($"Vehicle with ID {id} not found.");
            
            return Ok(vehicleWithHistory);
        }
    }
}