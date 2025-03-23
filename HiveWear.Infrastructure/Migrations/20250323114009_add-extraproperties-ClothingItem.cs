using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiveWear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addextrapropertiesClothingItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ClothingItems",
                newName: "ImagePath");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "ClothingItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateUpdated",
                table: "ClothingItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ClothingItems");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "ClothingItems",
                newName: "ImageUrl");
        }
    }
}
