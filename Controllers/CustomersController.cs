using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly DataService _dataService;

        public CustomersController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public ActionResult<List<Customer>> GetAll()
        {
            return Ok(_dataService.GetAllCustomers());
        }

        [HttpGet("{id}")]
        public ActionResult<Customer> GetById(int id)
        {
            var customer = _dataService.GetCustomerById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public ActionResult<Customer> Create([FromBody] Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName) || 
                string.IsNullOrWhiteSpace(customer.LastName) || 
                string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                return BadRequest("All fields are required");
            }

            var createdCustomer = _dataService.AddCustomer(customer);
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }

        // NEW: Get customer with all their repair orders
        [HttpGet("{id}/orders")]
        public ActionResult<CustomerWithOrdersDto> GetCustomerWithOrders(int id)
        {
            var customerWithOrders = _dataService.GetCustomerWithOrders(id);
            if (customerWithOrders == null)
                return NotFound($"Customer with ID {id} not found.");
            
            return Ok(customerWithOrders);
        }
    }
}