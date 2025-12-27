using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardSPs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Table Users removed from migration because it already exists in the database.
            
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 27, 12, 51, 18, 879, DateTimeKind.Utc).AddTicks(5965));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 27, 12, 51, 18, 879, DateTimeKind.Utc).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 27, 19, 51, 18, 879, DateTimeKind.Local).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 27, 12, 51, 18, 879, DateTimeKind.Utc).AddTicks(6105));

            // --- ADD STORED PROCEDURES ---

            var sp_GetDashboardSummary = @"
                CREATE PROCEDURE sp_GetDashboardSummary
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT
                        (SELECT COALESCE(SUM(TotalAmount), 0) FROM Transactions WHERE CAST(TransactionDate AS DATE) = CAST(GETDATE() AS DATE)) as TotalSalesToday,
                        (SELECT COUNT(*) FROM Transactions WHERE CAST(TransactionDate AS DATE) = CAST(GETDATE() AS DATE)) as TotalTransactionsToday,
                        (SELECT COUNT(*) FROM Products WHERE Active = 1) as TotalProducts,
                        (SELECT COUNT(*) FROM Customers WHERE DeletedAt IS NULL) as TotalCustomers;
                END";

            var sp_GetSalesChart = @"
                CREATE PROCEDURE sp_GetSalesChart
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @SevenDaysAgo DATE = DATEADD(DAY, -6, GETDATE());

                    SELECT 
                        FORMAT(TransactionDate, 'dd/MM/yyyy') as Date,
                        SUM(TotalAmount) as TotalAmount
                    FROM Transactions
                    WHERE CAST(TransactionDate AS DATE) >= @SevenDaysAgo
                    GROUP BY FORMAT(TransactionDate, 'dd/MM/yyyy'), CAST(TransactionDate AS DATE)
                    ORDER BY CAST(TransactionDate AS DATE) ASC;
                END";

            var sp_GetTopSellingProducts = @"
                CREATE PROCEDURE sp_GetTopSellingProducts
                    @Count INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SELECT TOP (@Count)
                        p.ProductName,
                        SUM(td.Quantity) as TotalQuantitySold,
                        SUM(td.SubTotal) as TotalRevenue
                    FROM TransactionDetails td
                    JOIN Products p ON td.ProductId = p.Id
                    GROUP BY p.ProductName
                    ORDER BY TotalQuantitySold DESC;
                END";

            migrationBuilder.Sql(sp_GetDashboardSummary);
            migrationBuilder.Sql(sp_GetSalesChart);
            migrationBuilder.Sql(sp_GetTopSellingProducts);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Stored Procedures
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetDashboardSummary");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetSalesChart");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_GetTopSellingProducts");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 14, 12, 28, 106, DateTimeKind.Utc).AddTicks(2337));

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 14, 12, 28, 106, DateTimeKind.Utc).AddTicks(2553));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 21, 12, 28, 106, DateTimeKind.Local).AddTicks(2532));

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 16, 14, 12, 28, 106, DateTimeKind.Utc).AddTicks(2509));
        }
    }
}
