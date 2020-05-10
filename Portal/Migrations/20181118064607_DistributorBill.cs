using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Commision",
                table: "ProductMasters",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Commision",
                table: "ProductMasters");
        }
    }
}
