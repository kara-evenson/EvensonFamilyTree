using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvensonFamilyTreeAppsDev.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStoriesAndAuthorizedViewers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Partnerships",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FamilyTrees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FamilyTrees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FamilyTrees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Story",
                table: "UserStories",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "UserStories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FamilyTreeId",
                table: "UserStories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PersonId1",
                table: "UserStories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserStories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "Occupations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Occupations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Commendations",
                table: "MilitaryServices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "MilitaryServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AuthorizedViewers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamilyTreeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedViewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizedViewers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthorizedViewers_FamilyTrees_FamilyTreeId",
                        column: x => x.FamilyTreeId,
                        principalTable: "FamilyTrees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<int>(type: "int", nullable: true),
                    SchoolAttended = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Education_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_FamilyTreeId",
                table: "UserStories",
                column: "FamilyTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_PersonId1",
                table: "UserStories",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_UserId",
                table: "UserStories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Occupations_PersonId",
                table: "Occupations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MilitaryServices_PersonId",
                table: "MilitaryServices",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedViewers_FamilyTreeId",
                table: "AuthorizedViewers",
                column: "FamilyTreeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizedViewers_UserId",
                table: "AuthorizedViewers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Education_PersonId",
                table: "Education",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_MilitaryServices_People_PersonId",
                table: "MilitaryServices",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_People_PersonId",
                table: "Occupations",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_AspNetUsers_UserId",
                table: "UserStories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_FamilyTrees_FamilyTreeId",
                table: "UserStories",
                column: "FamilyTreeId",
                principalTable: "FamilyTrees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_People_PersonId1",
                table: "UserStories",
                column: "PersonId1",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MilitaryServices_People_PersonId",
                table: "MilitaryServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupations_People_PersonId",
                table: "Occupations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_AspNetUsers_UserId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_FamilyTrees_FamilyTreeId",
                table: "UserStories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_People_PersonId1",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "AuthorizedViewers");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_FamilyTreeId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_PersonId1",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_UserId",
                table: "UserStories");

            migrationBuilder.DropIndex(
                name: "IX_Occupations_PersonId",
                table: "Occupations");

            migrationBuilder.DropIndex(
                name: "IX_MilitaryServices_PersonId",
                table: "MilitaryServices");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "FamilyTreeId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "PersonId1",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Occupations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Occupations");

            migrationBuilder.DropColumn(
                name: "Commendations",
                table: "MilitaryServices");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "MilitaryServices");

            migrationBuilder.AlterColumn<string>(
                name: "Story",
                table: "UserStories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000);

            migrationBuilder.InsertData(
                table: "FamilyTrees",
                columns: new[] { "Id", "FamilyName", "OwnerId" },
                values: new object[,]
                {
                    { 1, "Johnson Family Tree", null },
                    { 2, "Martinez Family Tree", null },
                    { 3, "Evenson Family Tree", null }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "BirthDate", "BirthPlace", "DeathDate", "FamilyTreeId", "FirstName", "Gender", "LastName", "LifeDescription", "Parent1Id", "Parent2Id", "Prefix", "RestingPlace", "Suffix" },
                values: new object[,]
                {
                    { 1, new DateTime(1970, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chicago, IL", null, 1, "Robert", 1, "Johnson", null, null, null, null, null, null },
                    { 2, new DateTime(1974, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boston, MA", null, 1, "Emily", 2, "Johnson", null, null, null, null, null, null },
                    { 4, new DateTime(1965, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "San Juan, PR", null, 2, "Carlos", 1, "Martinez", null, null, null, null, null, null },
                    { 5, new DateTime(1968, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Houston, TX", null, 2, "Ana", 2, "Martinez", null, null, null, null, null, null },
                    { 7, new DateTime(1994, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ashland, WI", null, 3, "Kara", 2, "Evenson", null, null, null, null, null, null },
                    { 8, new DateTime(1958, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ashland, WI", null, 3, "Brian", 1, "Evenson", null, null, null, null, null, null },
                    { 9, new DateTime(1958, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Florham Park, NJ", null, 3, "Kathleen", 2, "Evenson", null, null, null, null, null, null },
                    { 10, new DateTime(1932, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ashland, WI", null, 3, "Marlene", 2, "Martin", null, null, null, null, null, null },
                    { 11, new DateTime(1943, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grand Forks, ND", new DateTime(2021, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Gary", 1, "Martin", null, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Partnerships",
                columns: new[] { "Id", "EndDate", "Notes", "Person1Id", "Person2Id", "RelationshipTypeId", "StartDate" },
                values: new object[,]
                {
                    { 1, null, null, 1, 2, 1, new DateTime(1998, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, null, null, 4, 5, 1, new DateTime(1990, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, null, null, 8, 9, 1, new DateTime(1989, 8, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "BirthDate", "BirthPlace", "DeathDate", "FamilyTreeId", "FirstName", "Gender", "LastName", "LifeDescription", "Parent1Id", "Parent2Id", "Prefix", "RestingPlace", "Suffix" },
                values: new object[,]
                {
                    { 3, new DateTime(2001, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chicago, IL", null, 1, "Michael", 1, "Johnson", null, 1, 2, null, null, null },
                    { 6, new DateTime(1998, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Houston, TX", null, 2, "Sophia", 2, "Martinez", null, 4, 5, null, null, null }
                });
        }
    }
}
