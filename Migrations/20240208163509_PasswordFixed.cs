using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lesson1.Migrations
{
    /// <inheritdoc />
    public partial class PasswordFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passward",
                table: "User",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "User",
                newName: "Passward");
        }
    }
}
