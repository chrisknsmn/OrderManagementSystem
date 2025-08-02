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

        // NEW: Get customer with all their repair orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<CustomerWithOrdersDto>> GetCustomerWithOrders(int id)
        {
            var customerWithOrders = await _dataService.GetCustomerWithOrdersAsync(id);
            if (customerWithOrders == null)
                return NotFound($"Customer with ID {id} not found.");
            
            return Ok(customerWithOrders);
        }
    }
}