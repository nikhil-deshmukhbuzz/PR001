using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill_C : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "DistributorBills",
                newName: "ProductAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "PaybleAmount",
                table: "DistributorBills",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaybleAmount",
                table: "DistributorBills");

            migrationBuilder.RenameColumn(
                name: "ProductAmount",
                table: "DistributorBills",
                newName: "TotalAmount");
        }
    }
}
