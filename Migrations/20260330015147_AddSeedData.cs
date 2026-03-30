using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EvensonFamilyTreeAppsDev.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "RelationshipType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Married" },
                    { 2, "Engaged" },
                    { 3, "Divorced" },
                    { 4, "Partnered" }
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "RelationshipType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RelationshipType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RelationshipType",
                keyColumn: "Id",
                keyValue: 4);

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
                table: "RelationshipType",
                keyColumn: "Id",
                keyValue: 1);

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
        }
    }
}
