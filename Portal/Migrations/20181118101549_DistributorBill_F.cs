using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill_F : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceID",
                table: "DistributorBills",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills",
                column: "InvoiceID",
                principalTable: "Invoices",
                principalColumn: "InvoiceID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceID",
                table: "DistributorBills",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorBills_Invoices_InvoiceID",
                table: "DistributorBills",
                column: "InvoiceID",
                principalTable: "Invoices",
                principalColumn: "InvoiceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
