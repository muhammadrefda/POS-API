using POS_API.DTOs;

namespace POS_API.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<IEnumerable<SalesChartDto>> GetSalesLast7DaysAsync();
        Task<IEnumerable<TopProductDto>> GetTopSellingProductsAsync(int count);
    }
}
