using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreTM.Migrations
{
    /// <inheritdoc />
    public partial class AddTacGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Product",
                type: "nvarchar(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Product");
        }
    }
}
