using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvensonFamilyTreeAppsDev.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonHistoryRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_People_PersonId",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_People_PersonId1",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_PersonId1",
                table: "UserStories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Education",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "UserStories");

            migrationBuilder.RenameTable(
                name: "Education",
                newName: "Educations");

            migrationBuilder.RenameIndex(
                name: "IX_Education_PersonId",
                table: "Educations",
                newName: "IX_Educations_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Educations",
                table: "Educations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_People_PersonId",
                table: "Educations",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Educations_People_PersonId",
                table: "Educations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Educations",
                table: "Educations");

            migrationBuilder.RenameTable(
                name: "Educations",
                newName: "Education");

            migrationBuilder.RenameIndex(
                name: "IX_Educations_PersonId",
                table: "Education",
                newName: "IX_Education_PersonId");

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "UserStories",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Education",
                table: "Education",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_PersonId1",
                table: "UserStories",
                column: "PersonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_People_PersonId",
                table: "Education",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_People_PersonId1",
                table: "UserStories",
                column: "PersonId1",
                principalTable: "People",
                principalColumn: "Id");
        }
    }
}
