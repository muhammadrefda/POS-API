using Microsoft.EntityFrameworkCore;
using POS_API.DTOs;
using POS_API.Interfaces;

namespace POS_API.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            // EF Core 8 Raw SQL Query mapping to DTO
            var result = await _context.Database
                .SqlQuery<DashboardSummaryDto>($"EXEC sp_GetDashboardSummary")
                .ToListAsync();

            return result.FirstOrDefault() ?? new DashboardSummaryDto();
        }

        public async Task<IEnumerable<SalesChartDto>> GetSalesLast7DaysAsync()
        {
            return await _context.Database
                .SqlQuery<SalesChartDto>($"EXEC sp_GetSalesChart")
                .ToListAsync();
        }

        public async Task<IEnumerable<TopProductDto>> GetTopSellingProductsAsync(int count)
        {
            return await _context.Database
                .SqlQuery<TopProductDto>($"EXEC sp_GetTopSellingProducts {count}")
                .ToListAsync();
        }
    }
}