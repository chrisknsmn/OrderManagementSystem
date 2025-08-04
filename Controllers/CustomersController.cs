using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IDataService _dataService;

        public CustomersController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            var customers = await _dataService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(int id)
        {
            var customer = await _dataService.GetCustomerByIdAsync(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> Create([FromBody] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName) || 
                string.IsNullOrWhiteSpace(customer.LastName) || 
                string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                return BadRequest("All fields are required");
            }

            var createdCustomer = await _dataService.AddCustomerAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }


        // DELETE: Delete customer and all related repair orders
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var deleted = await _dataService.DeleteCustomerAsync(id);
            if (!deleted)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return NoContent();
        }
    }
}