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
        }
    }
}
