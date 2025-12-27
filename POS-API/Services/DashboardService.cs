using POS_API.DTOs;
using POS_API.Interfaces;

namespace POS_API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepo;

        public DashboardService(IDashboardRepository dashboardRepo)
        {
            _dashboardRepo = dashboardRepo;
        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            return await _dashboardRepo.GetDashboardSummaryAsync();
        }

        public async Task<IEnumerable<SalesChartDto>> GetSalesChartAsync()
        {
            return await _dashboardRepo.GetSalesLast7DaysAsync();
        }

        public async Task<IEnumerable<TopProductDto>> GetTopProductsAsync(int count)
        {
            return await _dashboardRepo.GetTopSellingProductsAsync(count);
        }
    }
}