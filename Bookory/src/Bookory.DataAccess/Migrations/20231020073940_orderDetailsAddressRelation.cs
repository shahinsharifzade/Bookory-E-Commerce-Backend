using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookory.DataAccess.Migrations
{
    public partial class orderDetailsAddressRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UseraddressId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_UserAddresses_UseraddressId",
                table: "OrderDetails",
                column: "UseraddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id");
        }
    }
}
