using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DispatchDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispatchDetails",
                columns: table => new
                {
                    DispatchDetailID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DispatchID = table.Column<long>(nullable: false),
                    InventoryID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchDetails", x => x.DispatchDetailID);
                    table.ForeignKey(
                        name: "FK_DispatchDetails_Dispatchs_DispatchID",
                        column: x => x.DispatchID,
                        principalTable: "Dispatchs",
                        principalColumn: "DispatchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispatchDetails_InventoryMasters_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "InventoryMasters",
                        principalColumn: "InventoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispatchDetails_DispatchID",
                table: "DispatchDetails",
                column: "DispatchID");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchDetails_InventoryID",
                table: "DispatchDetails",
                column: "InventoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatchDetails");
        }
    }
}
