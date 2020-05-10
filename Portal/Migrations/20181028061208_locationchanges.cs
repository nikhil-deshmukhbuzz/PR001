using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class locationchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistrictMasters_CityMasters_CityID",
                table: "DistrictMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_StateMasters_DistrictMasters_DistrictID",
                table: "StateMasters");

            migrationBuilder.DropIndex(
                name: "IX_StateMasters_DistrictID",
                table: "StateMasters");

            migrationBuilder.DropColumn(
                name: "DistrictID",
                table: "StateMasters");

            migrationBuilder.RenameColumn(
                name: "CityID",
                table: "DistrictMasters",
                newName: "StateID");

            migrationBuilder.RenameIndex(
                name: "IX_DistrictMasters_CityID",
                table: "DistrictMasters",
                newName: "IX_DistrictMasters_StateID");

            migrationBuilder.AddColumn<long>(
                name: "DistrictID",
                table: "CityMasters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CityMasters_DistrictID",
                table: "CityMasters",
                column: "DistrictID");

            migrationBuilder.AddForeignKey(
                name: "FK_CityMasters_DistrictMasters_DistrictID",
                table: "CityMasters",
                column: "DistrictID",
                principalTable: "DistrictMasters",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DistrictMasters_StateMasters_StateID",
                table: "DistrictMasters",
                column: "StateID",
                principalTable: "StateMasters",
                principalColumn: "StateID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityMasters_DistrictMasters_DistrictID",
                table: "CityMasters");

            migrationBuilder.DropForeignKey(
                name: "FK_DistrictMasters_StateMasters_StateID",
                table: "DistrictMasters");

            migrationBuilder.DropIndex(
                name: "IX_CityMasters_DistrictID",
                table: "CityMasters");

            migrationBuilder.DropColumn(
                name: "DistrictID",
                table: "CityMasters");

            migrationBuilder.RenameColumn(
                name: "StateID",
                table: "DistrictMasters",
                newName: "CityID");

            migrationBuilder.RenameIndex(
                name: "IX_DistrictMasters_StateID",
                table: "DistrictMasters",
                newName: "IX_DistrictMasters_CityID");

            migrationBuilder.AddColumn<long>(
                name: "DistrictID",
                table: "StateMasters",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StateMasters_DistrictID",
                table: "StateMasters",
                column: "DistrictID");

            migrationBuilder.AddForeignKey(
                name: "FK_DistrictMasters_CityMasters_CityID",
                table: "DistrictMasters",
                column: "CityID",
                principalTable: "CityMasters",
                principalColumn: "CityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StateMasters_DistrictMasters_DistrictID",
                table: "StateMasters",
                column: "DistrictID",
                principalTable: "DistrictMasters",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
