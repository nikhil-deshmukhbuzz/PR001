using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class UserMasterAlter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MenuMasters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMobile",
                table: "MenuMasters",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MenuMasters");

            migrationBuilder.DropColumn(
                name: "IsMobile",
                table: "MenuMasters");
        }
    }
}
