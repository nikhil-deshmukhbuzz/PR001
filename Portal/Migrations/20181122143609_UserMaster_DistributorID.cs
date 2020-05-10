using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class UserMaster_DistributorID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DistributorID",
                table: "UserMasters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMasters_DistributorID",
                table: "UserMasters",
                column: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMasters_Distributors_DistributorID",
                table: "UserMasters",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMasters_Distributors_DistributorID",
                table: "UserMasters");

            migrationBuilder.DropIndex(
                name: "IX_UserMasters_DistributorID",
                table: "UserMasters");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "UserMasters");
        }
    }
}
