using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
        Task<IEnumerable<SalesChartDto>> GetSalesChartAsync();
        Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int count);
    }
}
