using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace klc_one.Migrations
{
    public partial class ShoppinglistAddedBaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ShoppingList",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ShoppingList",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ShoppingList",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ShoppingList");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ShoppingList");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ShoppingList");
        }
    }
}
