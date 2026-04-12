using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Person> People => Set<Person>();
        public DbSet<FamilyTree> FamilyTrees => Set<FamilyTree>();
        public DbSet<Occupation> Occupations => Set<Occupation>();
        public DbSet<Education> Educations => Set<Education>();
        public DbSet<MilitaryService> MilitaryServices => Set<MilitaryService>();
        public DbSet<MilitaryType> MilitaryTypes => Set<MilitaryType>();
        public DbSet<Partnership> Partnerships => Set<Partnership>();
        public DbSet<UserStory> UserStories => Set<UserStory>();
        public DbSet<RelationshipType> RelationshipTypes => Set<RelationshipType>();
        public DbSet<AuthorizedViewer> AuthorizedViewers => Set<AuthorizedViewer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<FamilyTree>()
                .HasOne(ft => ft.Owner)
                .WithMany(u => u.OwnedFamilyTrees)
                .HasForeignKey(ft => ft.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserStory>()
                .HasOne(us => us.Person)
                .WithMany(p => p.Stories)
                .HasForeignKey(us => us.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserStory>()
                .HasOne(us => us.FamilyTree)
                .WithMany(ft => ft.UserStories)
                .HasForeignKey(us => us.FamilyTreeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserStory>()
                .HasOne(us => us.User)
                .WithMany()
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthorizedViewer>()
                .HasOne(av => av.FamilyTree)
                .WithMany(ft => ft.AuthorizedViewers)
                .HasForeignKey(av => av.FamilyTreeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuthorizedViewer>()
                .HasOne(av => av.User)
                .WithMany()
                .HasForeignKey(av => av.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthorizedViewer>()
                .HasIndex(av => new { av.FamilyTreeId, av.UserId })
                .IsUnique();

            modelBuilder.Entity<Occupation>()
                .HasOne(o => o.Person)
                .WithMany(p => p.Occupations)
                .HasForeignKey(o => o.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Education>()
                .HasOne(e => e.Person)
                .WithMany(p => p.Educations)
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MilitaryService>()
                .HasOne(ms => ms.Person)
                .WithMany(p => p.MilitaryServices)
                .HasForeignKey(ms => ms.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData.Seed(modelBuilder);
        }
    }
}