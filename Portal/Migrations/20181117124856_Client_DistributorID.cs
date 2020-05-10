using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class Client_DistributorID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DistributorID",
                table: "ClientMasters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientMasters_DistributorID",
                table: "ClientMasters",
                column: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientMasters_Distributors_DistributorID",
                table: "ClientMasters",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientMasters_Distributors_DistributorID",
                table: "ClientMasters");

            migrationBuilder.DropIndex(
                name: "IX_ClientMasters_DistributorID",
                table: "ClientMasters");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "ClientMasters");
        }
    }
}
