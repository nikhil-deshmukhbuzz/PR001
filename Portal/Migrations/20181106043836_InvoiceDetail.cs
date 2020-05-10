using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InvoiceDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_InvoiceHeaders_InvoiceHeaderID",
                table: "InvoiceDetails");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceHeaderID",
                table: "InvoiceDetails",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "DisapatchDetailID",
                table: "InvoiceDetails",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DispatchDetailID",
                table: "InvoiceDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_DispatchDetailID",
                table: "InvoiceDetails",
                column: "DispatchDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_DispatchDetails_DispatchDetailID",
                table: "InvoiceDetails",
                column: "DispatchDetailID",
                principalTable: "DispatchDetails",
                principalColumn: "DispatchDetailID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_InvoiceHeaders_InvoiceHeaderID",
                table: "InvoiceDetails",
                column: "InvoiceHeaderID",
                principalTable: "InvoiceHeaders",
                principalColumn: "InvoiceHeaderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_DispatchDetails_DispatchDetailID",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_InvoiceHeaders_InvoiceHeaderID",
                table: "InvoiceDetails");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceDetails_DispatchDetailID",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "DisapatchDetailID",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "DispatchDetailID",
                table: "InvoiceDetails");

            migrationBuilder.AlterColumn<long>(
                name: "InvoiceHeaderID",
                table: "InvoiceDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_InvoiceHeaders_InvoiceHeaderID",
                table: "InvoiceDetails",
                column: "InvoiceHeaderID",
                principalTable: "InvoiceHeaders",
                principalColumn: "InvoiceHeaderID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
