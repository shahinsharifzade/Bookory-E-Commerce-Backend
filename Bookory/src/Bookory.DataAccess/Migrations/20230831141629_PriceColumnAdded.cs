using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookory.DataAccess.Migrations
{
    public partial class PriceColumnAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total",
                table: "ShoppingSessions",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "BasketItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketItems");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "ShoppingSessions",
                newName: "Total");
        }
    }
}
