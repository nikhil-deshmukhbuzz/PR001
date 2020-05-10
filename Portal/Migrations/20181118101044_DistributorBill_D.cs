using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill_D : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceID",
                table: "DistributorBills",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_InvoiceID",
                table: "DistributorBills",
                column: "InvoiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills",
                column: "InvoiceID",
                principalTable: "Invoices",
                principalColumn: "InvoiceID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills");

            migrationBuilder.DropIndex(
                name: "IX_DistributorBills_InvoiceID",
                table: "DistributorBills");

            migrationBuilder.DropColumn(
                name: "InvoiceID",
                table: "DistributorBills");
        }
    }
}
