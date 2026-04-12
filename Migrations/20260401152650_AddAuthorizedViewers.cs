using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvensonFamilyTreeAppsDev.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorizedViewers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthorizedViewers_FamilyTreeId",
                table: "AuthorizedViewers");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedViewers_FamilyTreeId_UserId",
                table: "AuthorizedViewers",
                columns: new[] { "FamilyTreeId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthorizedViewers_FamilyTreeId_UserId",
                table: "AuthorizedViewers");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedViewers_FamilyTreeId",
                table: "AuthorizedViewers",
                column: "FamilyTreeId");
        }
    }
}
