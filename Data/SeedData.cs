using EvensonFamilyTreeAppsDev.Models;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RelationshipType>().HasData(
                new RelationshipType { Id = 1, Name = "Married" },
                new RelationshipType { Id = 2, Name = "Engaged" },
                new RelationshipType { Id = 3, Name = "Divorced" },
                new RelationshipType { Id = 4, Name = "Partnered" }
            );

            modelBuilder.Entity<FamilyTree>().HasData(
                new FamilyTree { Id = 1, FamilyName = "Johnson Family Tree" },
                new FamilyTree { Id = 2, FamilyName = "Martinez Family Tree" },
                new FamilyTree { Id = 3, FamilyName = "Evenson Family Tree" }
            );

            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, FirstName = "Robert", LastName = "Johnson", BirthDate = new DateTime(1970, 3, 15), BirthPlace = "Chicago, IL", Gender = GenderType.Male, FamilyTreeId = 1 },
                new Person { Id = 2, FirstName = "Emily", LastName = "Johnson", BirthDate = new DateTime(1974, 8, 10), BirthPlace = "Boston, MA", Gender = GenderType.Female, FamilyTreeId = 1 },
                new Person { Id = 3, FirstName = "Michael", LastName = "Johnson", BirthDate = new DateTime(2001, 5, 22), BirthPlace = "Chicago, IL", Gender = GenderType.Male, FamilyTreeId = 1, Parent1Id = 1, Parent2Id = 2 },
                new Person { Id = 4, FirstName = "Carlos", LastName = "Martinez", BirthDate = new DateTime(1965, 11, 12), BirthPlace = "San Juan, PR", Gender = GenderType.Male, FamilyTreeId = 2 },
                new Person { Id = 5, FirstName = "Ana", LastName = "Martinez", BirthDate = new DateTime(1968, 2, 18), BirthPlace = "Houston, TX", Gender = GenderType.Female, FamilyTreeId = 2 },
                new Person { Id = 6, FirstName = "Sophia", LastName = "Martinez", BirthDate = new DateTime(1998, 7, 30), BirthPlace = "Houston, TX", Gender = GenderType.Female, FamilyTreeId = 2, Parent1Id = 4, Parent2Id = 5 },
                new Person { Id = 7, FirstName = "Kara", LastName = "Evenson", BirthDate = new DateTime(1994, 4, 18), BirthPlace = "Ashland, WI", Gender = GenderType.Female, FamilyTreeId = 3 },
                new Person { Id = 8, FirstName = "Brian", LastName = "Evenson", BirthDate = new DateTime(1958, 3, 14), BirthPlace = "Ashland, WI", Gender = GenderType.Male, FamilyTreeId = 3 },
                new Person { Id = 9, FirstName = "Kathleen", LastName = "Evenson", BirthDate = new DateTime(1958, 4, 30), BirthPlace = "Florham Park, NJ", Gender = GenderType.Female, FamilyTreeId = 3 },
                new Person { Id = 10, FirstName = "Marlene", LastName = "Martin", BirthDate = new DateTime(1932, 5, 10), BirthPlace = "Ashland, WI", Gender = GenderType.Female, FamilyTreeId = 3 },
                new Person { Id = 11, FirstName = "Gary", LastName = "Martin", BirthDate = new DateTime(1943, 5, 15), DeathDate = new DateTime(2021, 8, 12), BirthPlace = "Grand Forks, ND", Gender = GenderType.Male, FamilyTreeId = 3 }
            );

            modelBuilder.Entity<Partnership>().HasData(
                new Partnership { Id = 1, Person1Id = 1, Person2Id = 2, RelationshipTypeId = 1, StartDate = new DateTime(1998, 6, 1) },
                new Partnership { Id = 2, Person1Id = 4, Person2Id = 5, RelationshipTypeId = 1, StartDate = new DateTime(1990, 4, 20) },
                new Partnership { Id = 3, Person1Id = 8, Person2Id = 9, RelationshipTypeId = 1, StartDate = new DateTime(1989, 8, 26) }
            );
        }
    }
}
