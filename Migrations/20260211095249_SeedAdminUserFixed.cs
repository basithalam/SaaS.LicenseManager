using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.LicenseManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminUsers",
                columns: new[] { "Id", "Email", "Password", "Username" },
                values: new object[] { 1, "admin@example.com", "$2a$11$qM/r6mUoI1B8zY.m6p7K.eR3H.o5v.m6p7K.eR3H.o5v.m6p7K.e", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
