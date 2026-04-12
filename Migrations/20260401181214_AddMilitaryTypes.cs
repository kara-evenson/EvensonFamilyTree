using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvensonFamilyTreeAppsDev.Migrations
{
    /// <inheritdoc />
    public partial class AddMilitaryTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MilitaryServices_MilitaryType_MilitaryTypeId",
                table: "MilitaryServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MilitaryType",
                table: "MilitaryType");

            migrationBuilder.RenameTable(
                name: "MilitaryType",
                newName: "MilitaryTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MilitaryTypes",
                table: "MilitaryTypes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "MilitaryTypes",
                columns: new[] { "Id", "MilitaryBranch" },
                values: new object[,]
                {
                    { 1, "Army" },
                    { 2, "Navy" },
                    { 3, "Air Force" },
                    { 4, "Marines" },
                    { 5, "Coast Guard" },
                    { 6, "Space Force" },
                    { 7, "National Guard" },
                    { 8, "Other" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_MilitaryServices_MilitaryTypes_MilitaryTypeId",
                table: "MilitaryServices",
                column: "MilitaryTypeId",
                principalTable: "MilitaryTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MilitaryServices_MilitaryTypes_MilitaryTypeId",
                table: "MilitaryServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MilitaryTypes",
                table: "MilitaryTypes");

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MilitaryTypes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.RenameTable(
                name: "MilitaryTypes",
                newName: "MilitaryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MilitaryType",
                table: "MilitaryType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MilitaryServices_MilitaryType_MilitaryTypeId",
                table: "MilitaryServices",
                column: "MilitaryTypeId",
                principalTable: "MilitaryType",
                principalColumn: "Id");
        }
    }
}
