using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.LicenseManager.Migrations
{
    /// <inheritdoc />
    public partial class AdminProfileAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "AdminUsers");
        }
    }
}
