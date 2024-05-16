using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreTM.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Customer_CustomerID",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Product_ProductId",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "Favorite");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_ProductId",
                table: "Favorite",
                newName: "IX_Favorite_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_CustomerID",
                table: "Favorite",
                newName: "IX_Favorite_CustomerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite",
                column: "FavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Customer_CustomerID",
                table: "Favorite",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Product_ProductId",
                table: "Favorite",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Customer_CustomerID",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Product_ProductId",
                table: "Favorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite");

            migrationBuilder.RenameTable(
                name: "Favorite",
                newName: "Favorites");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_ProductId",
                table: "Favorites",
                newName: "IX_Favorites_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_CustomerID",
                table: "Favorites",
                newName: "IX_Favorites_CustomerID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                column: "FavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Customer_CustomerID",
                table: "Favorites",
                column: "CustomerID",
                principalTable: "Customer",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Product_ProductId",
                table: "Favorites",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
