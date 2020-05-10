using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InvoiceDetail_D : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_DispatchDetails_DispatchDetailID",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "DispatchDetailID",
                table: "InvoiceDetails",
                newName: "InventoryID");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_DispatchDetailID",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_InventoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_InventoryMasters_InventoryID",
                table: "InvoiceDetails",
                column: "InventoryID",
                principalTable: "InventoryMasters",
                principalColumn: "InventoryID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_InventoryMasters_InventoryID",
                table: "InvoiceDetails");

            migrationBuilder.RenameColumn(
                name: "InventoryID",
                table: "InvoiceDetails",
                newName: "DispatchDetailID");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_InventoryID",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_DispatchDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_DispatchDetails_DispatchDetailID",
                table: "InvoiceDetails",
                column: "DispatchDetailID",
                principalTable: "DispatchDetails",
                principalColumn: "DispatchDetailID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
