using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InvoiceHeader_B : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "InvoiceHeaders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSoftware",
                table: "InvoiceHeaders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "IsSoftware",
                table: "InvoiceHeaders");
        }
    }
}
