using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookory.DataAccess.Migrations
{
    public partial class UpdateOrderLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Books_BookId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_Books_BookId1",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_ShoppingSession_SessionId",
                table: "BasketItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShoppingSession_ShoppingSessionId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingSession_AspNetUsers_UserId",
                table: "ShoppingSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingSession",
                table: "ShoppingSession");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItem",
                table: "BasketItem");

            migrationBuilder.DropIndex(
                name: "IX_BasketItem_BookId1",
                table: "BasketItem");

            migrationBuilder.DropColumn(
                name: "BookId1",
                table: "BasketItem");

            migrationBuilder.RenameTable(
                name: "ShoppingSession",
                newName: "ShoppingSessions");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "BasketItem",
                newName: "BasketItems");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingSession_UserId",
                table: "ShoppingSessions",
                newName: "IX_ShoppingSessions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ShoppingSessionId",
                table: "Orders",
                newName: "IX_Orders_ShoppingSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItem_SessionId",
                table: "BasketItems",
                newName: "IX_BasketItems_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItem_BookId",
                table: "BasketItems",
                newName: "IX_BasketItems_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingSessions",
                table: "ShoppingSessions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_To_Book_BookKey",
                table: "BasketItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_ShoppingSessions_SessionId",
                table: "BasketItems",
                column: "SessionId",
                principalTable: "ShoppingSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShoppingSessions_ShoppingSessionId",
                table: "Orders",
                column: "ShoppingSessionId",
                principalTable: "ShoppingSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingSessions_AspNetUsers_UserId",
                table: "ShoppingSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItem_To_Book_BookKey",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_ShoppingSessions_SessionId",
                table: "BasketItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShoppingSessions_ShoppingSessionId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingSessions_AspNetUsers_UserId",
                table: "ShoppingSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingSessions",
                table: "ShoppingSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems");

            migrationBuilder.RenameTable(
                name: "ShoppingSessions",
                newName: "ShoppingSession");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "BasketItems",
                newName: "BasketItem");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingSessions_UserId",
                table: "ShoppingSession",
                newName: "IX_ShoppingSession_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShoppingSessionId",
                table: "Order",
                newName: "IX_Order_ShoppingSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_SessionId",
                table: "BasketItem",
                newName: "IX_BasketItem_SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketItems_BookId",
                table: "BasketItem",
                newName: "IX_BasketItem_BookId");

            migrationBuilder.AddColumn<Guid>(
                name: "BookId1",
                table: "BasketItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingSession",
                table: "ShoppingSession",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItem",
                table: "BasketItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BookId1",
                table: "BasketItem",
                column: "BookId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Books_BookId",
                table: "BasketItem",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_Books_BookId1",
                table: "BasketItem",
                column: "BookId1",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItem_ShoppingSession_SessionId",
                table: "BasketItem",
                column: "SessionId",
                principalTable: "ShoppingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShoppingSession_ShoppingSessionId",
                table: "Order",
                column: "ShoppingSessionId",
                principalTable: "ShoppingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingSession_AspNetUsers_UserId",
                table: "ShoppingSession",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
