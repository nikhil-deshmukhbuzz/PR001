using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class DistributorBill_A : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistributorBills",
                columns: table => new
                {
                    DistributorBillID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillNumber = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    Commision = table.Column<decimal>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DistributorID = table.Column<long>(nullable: false),
                    ClientID = table.Column<long>(nullable: false),
                    ProductID = table.Column<long>(nullable: false),
                    PaymentModeID = table.Column<long>(nullable: true),
                    PaymentStatusID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorBills", x => x.DistributorBillID);
                    table.ForeignKey(
                        name: "FK_DistributorBills_ClientMasters_ClientID",
                        column: x => x.ClientID,
                        principalTable: "ClientMasters",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorBills_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorBills_PaymentModes_PaymentModeID",
                        column: x => x.PaymentModeID,
                        principalTable: "PaymentModes",
                        principalColumn: "PaymentModeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DistributorBills_PaymentStatuss_PaymentStatusID",
                        column: x => x.PaymentStatusID,
                        principalTable: "PaymentStatuss",
                        principalColumn: "PaymentStatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorBills_ProductMasters_ProductID",
                        column: x => x.ProductID,
                        principalTable: "ProductMasters",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_ClientID",
                table: "DistributorBills",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_DistributorID",
                table: "DistributorBills",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_PaymentModeID",
                table: "DistributorBills",
                column: "PaymentModeID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_PaymentStatusID",
                table: "DistributorBills",
                column: "PaymentStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorBills_ProductID",
                table: "DistributorBills",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorBills");
        }
    }
}
