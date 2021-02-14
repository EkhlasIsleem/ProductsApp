using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Product.DataAccess.Migrations
{
    public partial class initSO01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<string>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    Discription = table.Column<string>(nullable: true),
                    SOStatus = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    ShipmentDate = table.Column<DateTime>(nullable: true),
                    PhoneNo = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SOLines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SalesOrderId = table.Column<int>(nullable: false),
                    SalesOrdersId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SOLines_SalesOrders_SalesOrdersId",
                        column: x => x.SalesOrdersId,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SOLines_SalesOrdersId",
                table: "SOLines",
                column: "SalesOrdersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SOLines");

            migrationBuilder.DropTable(
                name: "SalesOrders");
        }
    }
}
