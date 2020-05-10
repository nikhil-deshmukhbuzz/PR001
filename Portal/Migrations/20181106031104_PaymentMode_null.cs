using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class PaymentMode_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PaymentModes_PaymentModeID",
                table: "Invoices");

            migrationBuilder.AlterColumn<long>(
                name: "PaymentModeID",
                table: "Invoices",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PaymentModes_PaymentModeID",
                table: "Invoices",
                column: "PaymentModeID",
                principalTable: "PaymentModes",
                principalColumn: "PaymentModeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_PaymentModes_PaymentModeID",
                table: "Invoices");

            migrationBuilder.AlterColumn<long>(
                name: "PaymentModeID",
                table: "Invoices",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_PaymentModes_PaymentModeID",
                table: "Invoices",
                column: "PaymentModeID",
                principalTable: "PaymentModes",
                principalColumn: "PaymentModeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
