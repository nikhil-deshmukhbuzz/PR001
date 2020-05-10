using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InvoiceHeader_C : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductID",
                table: "InvoiceHeaders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceHeaders_ProductID",
                table: "InvoiceHeaders",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceHeaders_ProductMasters_ProductID",
                table: "InvoiceHeaders",
                column: "ProductID",
                principalTable: "ProductMasters",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceHeaders_ProductMasters_ProductID",
                table: "InvoiceHeaders");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceHeaders_ProductID",
                table: "InvoiceHeaders");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "InvoiceHeaders");
        }
    }
}
