using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Data
{
    public static class IdentitySeedData
    {
        public static async Task SeedUsersAsync(
            UserManager<AppUser> userManager,
            ApplicationDbContext context)
        {
            await CreateUserWithTreeAndPeopleAsync(
                userManager, context,
                "johnson@test.com",
                "Johnson Family Tree");

            await CreateUserWithTreeAndPeopleAsync(
                userManager, context,
                "martinez@test.com",
                "Martinez Family Tree");

            await CreateUserWithTreeAndPeopleAsync(
                userManager, context,
                "evenson@test.com",
                "Evenson Family Tree");
        }

        private static async Task CreateUserWithTreeAndPeopleAsync(
            UserManager<AppUser> userManager,
            ApplicationDbContext context,
            string email,
            string familyName)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            AppUser user;

            if (existingUser == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Password123!");

                if (!result.Succeeded)
                {
                    return;
                }
            }
            else
            {
                user = existingUser;
            }

            var existingTree = await context.FamilyTrees
                .FirstOrDefaultAsync(ft => ft.OwnerId == user.Id);

            if (existingTree == null)
            {
                var familyTree = new FamilyTree
                {
                    FamilyName = familyName,
                    OwnerId = user.Id
                };

                context.FamilyTrees.Add(familyTree);
                await context.SaveChangesAsync();

                await SeedPeopleForTreeAsync(context, familyTree);
            }
            else
            {
                var hasPeople = await context.People.AnyAsync(p => p.FamilyTreeId == existingTree.Id);

                if (!hasPeople)
                {
                    await SeedPeopleForTreeAsync(context, existingTree);
                }
            }
        }

        private static async Task SeedPeopleForTreeAsync(ApplicationDbContext context, FamilyTree familyTree)
        {
            if (familyTree.FamilyName!.Contains("Johnson"))
            {
                var robert = new Person
                {
                    FirstName = "Robert",
                    LastName = "Johnson",
                    BirthDate = new DateTime(1970, 3, 15),
                    BirthPlace = "Chicago, IL",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                var emily = new Person
                {
                    FirstName = "Emily",
                    LastName = "Johnson",
                    BirthDate = new DateTime(1974, 8, 10),
                    BirthPlace = "Boston, MA",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id
                };

                context.People.Add(robert);
                context.People.Add(emily);
                await context.SaveChangesAsync();

                var michael = new Person
                {
                    FirstName = "Michael",
                    LastName = "Johnson",
                    BirthDate = new DateTime(2001, 5, 22),
                    BirthPlace = "Chicago, IL",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = robert.Id,
                    Parent2Id = emily.Id
                };

                context.People.Add(michael);
                await context.SaveChangesAsync();
            }
            else if (familyTree.FamilyName.Contains("Martinez"))
            {
                var carlos = new Person
                {
                    FirstName = "Carlos",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1965, 11, 12),
                    BirthPlace = "San Juan, PR",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                var ana = new Person
                {
                    FirstName = "Ana",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1968, 2, 18),
                    BirthPlace = "Houston, TX",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id
                };

                context.People.Add(carlos);
                context.People.Add(ana);
                await context.SaveChangesAsync();

                var sophia = new Person
                {
                    FirstName = "Sophia",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1998, 7, 30),
                    BirthPlace = "Houston, TX",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = carlos.Id,
                    Parent2Id = ana.Id
                };

                context.People.Add(sophia);
                await context.SaveChangesAsync();
            }
            else if (familyTree.FamilyName.Contains("Evenson"))
            {
                var brian = new Person
                {
                    FirstName = "Brian",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1958, 3, 14),
                    BirthPlace = "Ashland, WI",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                var kathleen = new Person
                {
                    FirstName = "Kathleen",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1958, 4, 30),
                    BirthPlace = "Florham Park, NJ",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id
                };

                context.People.Add(brian);
                context.People.Add(kathleen);
                await context.SaveChangesAsync();

                var kara = new Person
                {
                    FirstName = "Kara",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1994, 4, 18),
                    BirthPlace = "Ashland, WI",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = brian.Id,
                    Parent2Id = kathleen.Id
                };

                context.People.Add(kara);
                await context.SaveChangesAsync();
            }
        }
    }
}
