using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InvoiceHeader_A : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequenceNo",
                table: "InvoiceHeaders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceNo",
                table: "InvoiceHeaders");
        }
    }
}
