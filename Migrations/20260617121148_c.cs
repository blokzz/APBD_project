using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APBD_PROJEKT.Migrations
{
    /// <inheritdoc />
    public partial class c : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$gKSs4tyRgaghlPGq24qNxeJJ8Cz6tzRQbjA6erAWxURs3mZ2Xt4Y2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$wLRj5pQICLViNa7e99jtUOA1bn2VD1yIufwwE9/irCHwc3HNamxO.");
        }
    }
}
