using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GENTRY.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class Addtagstoitemtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Items",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Items");
        }
    }
}
