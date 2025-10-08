using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GENTRY.WebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddtableOccasionandWeather : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Occasion",
                table: "Outfits");

            migrationBuilder.DropColumn(
                name: "WeatherCondition",
                table: "Outfits");

            migrationBuilder.AddColumn<int>(
                name: "OccasionId",
                table: "Outfits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeatherId",
                table: "Outfits",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Occasions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weathers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weathers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outfits_OccasionId",
                table: "Outfits",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_Outfits_WeatherId",
                table: "Outfits",
                column: "WeatherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outfits_Occasions_OccasionId",
                table: "Outfits",
                column: "OccasionId",
                principalTable: "Occasions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Outfits_Weathers_WeatherId",
                table: "Outfits",
                column: "WeatherId",
                principalTable: "Weathers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outfits_Occasions_OccasionId",
                table: "Outfits");

            migrationBuilder.DropForeignKey(
                name: "FK_Outfits_Weathers_WeatherId",
                table: "Outfits");

            migrationBuilder.DropTable(
                name: "Occasions");

            migrationBuilder.DropTable(
                name: "Weathers");

            migrationBuilder.DropIndex(
                name: "IX_Outfits_OccasionId",
                table: "Outfits");

            migrationBuilder.DropIndex(
                name: "IX_Outfits_WeatherId",
                table: "Outfits");

            migrationBuilder.DropColumn(
                name: "OccasionId",
                table: "Outfits");

            migrationBuilder.DropColumn(
                name: "WeatherId",
                table: "Outfits");

            migrationBuilder.AddColumn<string>(
                name: "Occasion",
                table: "Outfits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeatherCondition",
                table: "Outfits",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
