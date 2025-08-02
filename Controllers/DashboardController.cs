using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly DataService _dataService;

        public DashboardController(DataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("statistics")]
        public ActionResult<object> GetSummaryStatistics()
        {
            var stats = _dataService.GetSummaryStatistics();
            return Ok(stats);
        }
    }
}