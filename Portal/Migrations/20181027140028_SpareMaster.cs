using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Portal.Migrations
{
    public partial class SpareMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "SpareMasters",
            columns: table => new
            {
                SpareID = table.Column<long>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Stock = table.Column<int>(nullable: false),
                SpareName = table.Column<string>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SpareMasters", x => x.SpareID);
            });

            //migrationBuilder.CreateTable(
            //    name: "DeviceMasters",
            //    columns: table => new
            //    {
            //        DeviceID = table.Column<long>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Stock = table.Column<int>(nullable: false),
            //        DeviceName = table.Column<string>(nullable: true),
            //        HardwareType = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DeviceMasters", x => x.DeviceID);
            //    });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMasters");

            migrationBuilder.DropTable(
                name: "SpareMasters");
        }
    }
}
