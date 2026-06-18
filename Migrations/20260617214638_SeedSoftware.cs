using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBD_PROJEKT.Migrations
{
    /// <inheritdoc />
    public partial class SeedSoftware : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "Category", "CurrentVersion", "Description", "Name", "YearlyLicensePrice" },
                values: new object[,]
                {
                    { 1, "Finanse", "1.0", "System finansowy", "FinanceApp", 5000m },
                    { 2, "Edukacja", "2.1", "Platforma edukacyjna", "EduPlatform", 3000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
