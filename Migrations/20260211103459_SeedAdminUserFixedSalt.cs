using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaS.LicenseManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUserFixedSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$abcdefghijklmnopqrstuufpm3fC6z8TMjQTb3Y/mPpJtrm0ki.PG");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$qM/r6mUoI1B8zY.m6p7K.eR3H.o5v.m6p7K.eR3H.o5v.m6p7K.e");
        }
    }
}
