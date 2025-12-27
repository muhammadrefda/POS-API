namespace POS_API.DTOs
{
    public class DashboardSummaryDto
    {
        public decimal TotalSalesToday { get; set; }
        public int TotalTransactionsToday { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
    }

    public class SalesChartDto
    {
        public string Date { get; set; } // Format: dd/MM/yyyy
        public decimal TotalAmount { get; set; }
    }

    public class TopProductDto
    {
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
