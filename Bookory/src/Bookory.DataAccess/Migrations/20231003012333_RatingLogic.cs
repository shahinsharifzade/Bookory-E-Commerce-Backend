using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookory.DataAccess.Migrations
{
    public partial class RatingLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRatings",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "NumberOfRatings",
                table: "Books");
        }
    }
}
