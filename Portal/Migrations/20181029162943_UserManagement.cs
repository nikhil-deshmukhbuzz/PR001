using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class UserManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuMasters",
                columns: table => new
                {
                    MenuID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MenuName = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    SequenceNo = table.Column<int>(nullable: false),
                    ParentMenuID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuMasters", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "ProfileMasters",
                columns: table => new
                {
                    ProfileID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileMasters", x => x.ProfileID);
                });

            migrationBuilder.CreateTable(
                name: "MenuProfileLinks",
                columns: table => new
                {
                    MenuProfileLinkID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProfileID = table.Column<long>(nullable: false),
                    MenuID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuProfileLinks", x => x.MenuProfileLinkID);
                    table.ForeignKey(
                        name: "FK_MenuProfileLinks_MenuMasters_MenuID",
                        column: x => x.MenuID,
                        principalTable: "MenuMasters",
                        principalColumn: "MenuID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuProfileLinks_ProfileMasters_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "ProfileMasters",
                        principalColumn: "ProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserMasters",
                columns: table => new
                {
                    UserID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    OTP = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsMobile = table.Column<bool>(nullable: false),
                    ProfileID = table.Column<long>(nullable: false),
                    ClientID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMasters", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserMasters_ClientMasters_ClientID",
                        column: x => x.ClientID,
                        principalTable: "ClientMasters",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserMasters_ProfileMasters_ProfileID",
                        column: x => x.ProfileID,
                        principalTable: "ProfileMasters",
                        principalColumn: "ProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuProfileLinks_MenuID",
                table: "MenuProfileLinks",
                column: "MenuID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuProfileLinks_ProfileID",
                table: "MenuProfileLinks",
                column: "ProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_UserMasters_ClientID",
                table: "UserMasters",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_UserMasters_ProfileID",
                table: "UserMasters",
                column: "ProfileID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuProfileLinks");

            migrationBuilder.DropTable(
                name: "UserMasters");

            migrationBuilder.DropTable(
                name: "MenuMasters");

            migrationBuilder.DropTable(
                name: "ProfileMasters");
        }
    }
}
