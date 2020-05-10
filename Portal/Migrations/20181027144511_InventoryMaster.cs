using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class InventoryMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryMasters",
                columns: table => new
                {
                    InventoryID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InventoryName = table.Column<string>(nullable: true),
                    InventoryType = table.Column<string>(nullable: true),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    WarrantyDate = table.Column<DateTime>(nullable: true),
                    DeviceID = table.Column<long>(nullable: true),
                    SpareID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMasters", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_InventoryMasters_DeviceMasters_DeviceID",
                        column: x => x.DeviceID,
                        principalTable: "DeviceMasters",
                        principalColumn: "DeviceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryMasters_SpareMasters_SpareID",
                        column: x => x.SpareID,
                        principalTable: "SpareMasters",
                        principalColumn: "SpareID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMasters_DeviceID",
                table: "InventoryMasters",
                column: "DeviceID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMasters_SpareID",
                table: "InventoryMasters",
                column: "SpareID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryMasters");
        }
    }
}
