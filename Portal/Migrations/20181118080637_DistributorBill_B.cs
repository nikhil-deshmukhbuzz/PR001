using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill_B : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderID",
                table: "DistributorBills",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_OrderID",
                table: "DistributorBills",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorBills_Orders_OrderID",
                table: "DistributorBills",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorBills_Orders_OrderID",
                table: "DistributorBills");

            migrationBuilder.DropIndex(
                name: "IX_DistributorBills_OrderID",
                table: "DistributorBills");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "DistributorBills");
        }
    }
}
