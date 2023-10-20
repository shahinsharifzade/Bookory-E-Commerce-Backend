using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookory.DataAccess.Migrations
{
    public partial class UpdateDiscountPriceColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "discountPrice",
                table: "Books",
                newName: "DiscountPercentage");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscountPercentage",
                table: "Books",
                newName: "discountPrice");
        }
    }
}
