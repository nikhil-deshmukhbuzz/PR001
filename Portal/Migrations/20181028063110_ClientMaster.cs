using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class ClientMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientMasters",
                columns: table => new
                {
                    ClientID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientName = table.Column<string>(nullable: true),
                    ClientCode = table.Column<string>(nullable: true),
                    ContactPerson = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CityID = table.Column<long>(nullable: false),
                    DistrictID = table.Column<long>(nullable: false),
                    StateID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientMasters", x => x.ClientID);
                    table.ForeignKey(
                        name: "FK_ClientMasters_CityMasters_CityID",
                        column: x => x.CityID,
                        principalTable: "CityMasters",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientMasters_DistrictMasters_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "DistrictMasters",
                        principalColumn: "DistrictID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientMasters_StateMasters_StateID",
                        column: x => x.StateID,
                        principalTable: "StateMasters",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientMasters_CityID",
                table: "ClientMasters",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMasters_DistrictID",
                table: "ClientMasters",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_ClientMasters_StateID",
                table: "ClientMasters",
                column: "StateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientMasters");
        }
    }
}
