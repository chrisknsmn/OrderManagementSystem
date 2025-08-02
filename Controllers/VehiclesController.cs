using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly DataService _dataService;

        public VehiclesController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public ActionResult<List<Vehicle>> GetAll()
        {
            return Ok(_dataService.GetAllVehicles());
        }

        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetById(int id)
        {
            var vehicle = _dataService.GetVehicleById(id);
            if (vehicle == null)
                return NotFound();
            return Ok(vehicle);
        }

        [HttpPost]
        public ActionResult<Vehicle> Create([FromBody] Vehicle vehicle)
        {
            if (vehicle.Year < 1900 || vehicle.Year > DateTime.Now.Year + 1 ||
                string.IsNullOrWhiteSpace(vehicle.Make) || 
                string.IsNullOrWhiteSpace(vehicle.Model))
            {
                return BadRequest("Invalid vehicle data");
            }

            var createdVehicle = _dataService.AddVehicle(vehicle);
            return CreatedAtAction(nameof(GetById), new { id = createdVehicle.Id }, createdVehicle);
        }
    }
}