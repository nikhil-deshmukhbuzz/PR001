using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class Dispatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dispatchs",
                columns: table => new
                {
                    DispatchID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DispatchDate = table.Column<DateTime>(nullable: true),
                    ShippingAddress = table.Column<string>(nullable: true),
                    DispatchNumber = table.Column<string>(nullable: true),
                    IsDispatched = table.Column<bool>(nullable: false),
                    ClientID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    OrderID = table.Column<long>(nullable: false),
                    CityID = table.Column<long>(nullable: false),
                    DistrictID = table.Column<long>(nullable: false),
                    StateID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispatchs", x => x.DispatchID);
                    table.ForeignKey(
                        name: "FK_Dispatchs_CityMasters_CityID",
                        column: x => x.CityID,
                        principalTable: "CityMasters",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispatchs_ClientMasters_ClientID",
                        column: x => x.ClientID,
                        principalTable: "ClientMasters",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispatchs_DistrictMasters_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "DistrictMasters",
                        principalColumn: "DistrictID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dispatchs_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dispatchs_ProductMasters_ProductID",
                        column: x => x.ProductID,
                        principalTable: "ProductMasters",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispatchs_StateMasters_StateID",
                        column: x => x.StateID,
                        principalTable: "StateMasters",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_CityID",
                table: "Dispatchs",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_ClientID",
                table: "Dispatchs",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_DistrictID",
                table: "Dispatchs",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_OrderID",
                table: "Dispatchs",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_ProductID",
                table: "Dispatchs",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatchs_StateID",
                table: "Dispatchs",
                column: "StateID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dispatchs");
        }
    }
}
