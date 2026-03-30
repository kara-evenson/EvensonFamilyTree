using EvensonFamilyTreeAppsDev.Models;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Person> People => Set<Person>();
        public DbSet<AppUser> Users => Set<AppUser>();
        public DbSet<FamilyTree> FamilyTrees => Set<FamilyTree>();

        public DbSet<Occupation> Occupations => Set<Occupation>();

        public DbSet<MilitaryService> MilitaryServices => Set<MilitaryService>();

        public DbSet<Partnership> Partnerships => Set<Partnership>();
        public DbSet<UserStory> UserStories => Set<UserStory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Self-referencing parents
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Parent1)
                .WithMany(p => p.ChildrenFromParent1)
                .HasForeignKey(p => p.Parent1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Parent2)
                .WithMany(p => p.ChildrenFromParent2)
                .HasForeignKey(p => p.Parent2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Partnership relationships
            modelBuilder.Entity<Partnership>()
                .HasOne(p => p.Person1)
                .WithMany()
                .HasForeignKey(p => p.Person1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Partnership>()
                .HasOne(p => p.Person2)
                .WithMany()
                .HasForeignKey(p => p.Person2Id)
                .OnDelete(DeleteBehavior.Restrict);

            SeedData.Seed(modelBuilder);
        }
    }

}
