using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class CityMaster_StateID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StateID",
                table: "CityMasters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CityMasters_StateID",
                table: "CityMasters",
                column: "StateID");

            migrationBuilder.AddForeignKey(
                name: "FK_CityMasters_StateMasters_StateID",
                table: "CityMasters",
                column: "StateID",
                principalTable: "StateMasters",
                principalColumn: "StateID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityMasters_StateMasters_StateID",
                table: "CityMasters");

            migrationBuilder.DropIndex(
                name: "IX_CityMasters_StateID",
                table: "CityMasters");

            migrationBuilder.DropColumn(
                name: "StateID",
                table: "CityMasters");
        }
    }
}
