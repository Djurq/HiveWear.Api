using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiveWear.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNullableUserToClothingItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ClothingItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClothingItems_UserId",
                table: "ClothingItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClothingItems_AspNetUsers_UserId",
                table: "ClothingItems");

            migrationBuilder.DropIndex(
                name: "IX_ClothingItems_UserId",
                table: "ClothingItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ClothingItems");
        }
    }
}
