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

            modelBuilder.Entity<MilitaryType>().HasData(
                new MilitaryType { Id = 1, MilitaryBranch = "Army" },
                new MilitaryType { Id = 2, MilitaryBranch = "Navy" },
                new MilitaryType { Id = 3, MilitaryBranch = "Air Force" },
                new MilitaryType { Id = 4, MilitaryBranch = "Marines" },
                new MilitaryType { Id = 5, MilitaryBranch = "Coast Guard" },
                new MilitaryType { Id = 6, MilitaryBranch = "Space Force" },
                new MilitaryType { Id = 7, MilitaryBranch = "National Guard" },
                new MilitaryType { Id = 8, MilitaryBranch = "Other" }
            );
        }
    }
}
