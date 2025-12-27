using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Categories",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Active = table.Column<bool>(type: "bit", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Categories", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Customers",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Customers", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Tags",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TagName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Tags", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Products",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        Stock = table.Column<int>(type: "int", nullable: false),
            //        Active = table.Column<bool>(type: "bit", nullable: false),
            //        CategoryId = table.Column<long>(type: "bigint", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Products", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Products_Categories_CategoryId",
            //            column: x => x.CategoryId,
            //            principalTable: "Categories",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Transactions",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        CreatedBy = table.Column<long>(type: "bigint", nullable: false),
            //        CustomerId = table.Column<long>(type: "bigint", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Transactions", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Transactions_Customers_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Customers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ProductTags",
            //    columns: table => new
            //    {
            //        ProductId = table.Column<long>(type: "bigint", nullable: false),
            //        TagId = table.Column<long>(type: "bigint", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProductTags", x => new { x.ProductId, x.TagId });
            //        table.ForeignKey(
            //            name: "FK_ProductTags_Products_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_ProductTags_Tags_TagId",
            //            column: x => x.TagId,
            //            principalTable: "Tags",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TransactionDetails",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Quantity = table.Column<int>(type: "int", nullable: false),
            //        UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
            //        TransactionId = table.Column<long>(type: "bigint", nullable: false),
            //        ProductId = table.Column<long>(type: "bigint", nullable: false),
            //        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TransactionDetails", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_TransactionDetails_Products_ProductId",
            //            column: x => x.ProductId,
            //            principalTable: "Products",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_TransactionDetails_Transactions_TransactionId",
            //            column: x => x.TransactionId,
            //            principalTable: "Transactions",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.InsertData(
            //    table: "Categories",
            //    columns: new[] { "Id", "Active", "CategoryName", "CreatedAt", "DeletedAt", "Description", "UpdatedAt" },
            //    values: new object[] { 1L, true, "Makanan ringan", new DateTime(2025, 11, 16, 14, 0, 49, 33, DateTimeKind.Utc).AddTicks(8306), null, null, null });

            //migrationBuilder.InsertData(
            //    table: "Customers",
            //    columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "FullName", "PhoneNumber", "UpdatedAt" },
            //    values: new object[] { 1L, new DateTime(2025, 11, 16, 14, 0, 49, 33, DateTimeKind.Utc).AddTicks(8574), null, "budi@example.com", "Budi Santoso", null, null });

            //migrationBuilder.InsertData(
            //    table: "Tags",
            //    columns: new[] { "Id", "CreatedAt", "DeletedAt", "TagName", "UpdatedAt" },
            //    values: new object[] { 1L, new DateTime(2025, 11, 16, 14, 0, 49, 33, DateTimeKind.Utc).AddTicks(8509), null, "Best Seller", null });

            //migrationBuilder.InsertData(
            //    table: "Products",
            //    columns: new[] { "Id", "Active", "CategoryId", "CreatedAt", "DeletedAt", "Price", "ProductName", "Stock", "UpdatedAt" },
            //    values: new object[] { 1L, true, 1L, new DateTime(2025, 11, 16, 21, 0, 49, 33, DateTimeKind.Local).AddTicks(8550), null, 15000m, "Keripik Kentang Original", 100, null });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Products_CategoryId",
            //    table: "Products",
            //    column: "CategoryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProductTags_TagId",
            //    table: "ProductTags",
            //    column: "TagId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TransactionDetails_ProductId",
            //    table: "TransactionDetails",
            //    column: "ProductId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TransactionDetails_TransactionId",
            //    table: "TransactionDetails",
            //    column: "TransactionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Transactions_CustomerId",
            //    table: "Transactions",
            //    column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "TransactionDetails");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
