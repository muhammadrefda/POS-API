using Microsoft.AspNetCore.Mvc;
using POS_API.Helpers;
using POS_API.Interfaces;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardApiController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _dashboardService.GetSummaryAsync();
            return Ok(new ApiResponse<object>(summary, "Dashboard summary retrieved successfully"));
        }

        [HttpGet("sales-chart")]
        public async Task<IActionResult> GetSalesChart()
        {
            var data = await _dashboardService.GetSalesChartAsync();
            return Ok(new ApiResponse<object>(data, "Sales chart data retrieved successfully"));
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts([FromQuery] int count = 5)
        {
            var data = await _dashboardService.GetTopProductsAsync(count);
            return Ok(new ApiResponse<object>(data, "Top selling products retrieved successfully"));
        }
    }
}
