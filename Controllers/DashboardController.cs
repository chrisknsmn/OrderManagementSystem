using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDataService _dataService;

        public DashboardController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetSummaryStatistics()
        {
            var stats = await _dataService.GetSummaryStatisticsAsync();
            return Ok(stats);
        }
    }
}