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

                await SeedPeopleForTreeAsync(context, familyTree, user.Id);
            }
            else
            {
                var hasPeople = await context.People.AnyAsync(p => p.FamilyTreeId == existingTree.Id);

                if (!hasPeople)
                {
                    await SeedPeopleForTreeAsync(context, existingTree, user.Id);
                }
            }
        }

        private static async Task SeedPeopleForTreeAsync(ApplicationDbContext context, FamilyTree familyTree, string userId)
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
                // ===== GENERATION 1 =====
                var rafael = new Person
                {
                    FirstName = "Rafael",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1940, 4, 12),
                    BirthPlace = "San Juan, PR",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    ImagePath = "/images/ancestors/martinez-rafael.jpg"
                };

                var lucia = new Person
                {
                    FirstName = "Lucia",
                    LastName = "Rivera",
                    BirthDate = new DateTime(1943, 9, 22),
                    BirthPlace = "Ponce, PR",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    ImagePath = "/images/ancestors/martinez-lucia.jpg"
                };

                var miguel = new Person
                {
                    FirstName = "Miguel",
                    LastName = "Santos",
                    BirthDate = new DateTime(1938, 1, 18),
                    BirthPlace = "Mayagüez, PR",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                context.People.AddRange(rafael, lucia, miguel);
                await context.SaveChangesAsync();

                // ===== GENERATION 2 =====
                var carlos = new Person
                {
                    FirstName = "Carlos",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1965, 11, 12),
                    BirthPlace = "San Juan, PR",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = rafael.Id,
                    Parent2Id = lucia.Id,
                    ImagePath = "/images/ancestors/martinez-carlos.jpg"
                };

                var elena = new Person
                {
                    FirstName = "Elena",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1969, 3, 8),
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = rafael.Id,
                    Parent2Id = lucia.Id,
                    ImagePath = "/images/ancestors/martinez-elena.jpg"
                };

                var ana = new Person
                {
                    FirstName = "Ana",
                    LastName = "Santos",
                    BirthDate = new DateTime(1968, 2, 18),
                    BirthPlace = "Houston, TX",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = miguel.Id,
                    ImagePath = "/images/ancestors/martinez-ana.jpg"
                };

                var victor = new Person
                {
                    FirstName = "Victor",
                    LastName = "Reyes",
                    BirthDate = new DateTime(1966, 6, 4),
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                context.People.AddRange(carlos, elena, ana, victor);
                await context.SaveChangesAsync();

                // ===== GENERATION 3 =====
                var sofia = new Person
                {
                    FirstName = "Sofia",
                    LastName = "Martinez",
                    BirthDate = new DateTime(1998, 7, 30),
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = carlos.Id,
                    Parent2Id = ana.Id,
                    ImagePath = "/images/ancestors/martinez-sofia.jpg"
                };

                var diego = new Person
                {
                    FirstName = "Diego",
                    LastName = "Martinez",
                    BirthDate = new DateTime(2001, 5, 15),
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = carlos.Id,
                    Parent2Id = ana.Id,
                    ImagePath = "/images/ancestors/martinez-diego.jpg"
                };

                var maya = new Person
                {
                    FirstName = "Maya",
                    LastName = "Reyes",
                    BirthDate = new DateTime(1993, 12, 3),
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = victor.Id,
                    Parent2Id = ana.Id,
                    ImagePath = "/images/ancestors/martinez-maya.jpg"
                };

                context.People.AddRange(sofia, diego, maya);
                await context.SaveChangesAsync();

                // ===== PARTNERSHIPS =====
                context.Partnerships.AddRange(
                    new Partnership { Person1Id = rafael.Id, Person2Id = lucia.Id, RelationshipTypeId = 1 },
                    new Partnership { Person1Id = ana.Id, Person2Id = victor.Id, RelationshipTypeId = 3, EndDate = new DateTime(1996, 1, 1) },
                    new Partnership { Person1Id = carlos.Id, Person2Id = ana.Id, RelationshipTypeId = 1 }
                );

                // ===== OCCUPATIONS =====
                context.Occupations.AddRange(
                    new Occupation { PersonId = carlos.Id, Title = "Logistics Coordinator" },
                    new Occupation { PersonId = ana.Id, Title = "Registered Nurse" },
                    new Occupation { PersonId = elena.Id, Title = "School Counselor" }
                );

                // ===== EDUCATION =====
                context.Educations.AddRange(
                    new Education { PersonId = ana.Id, EducationLevel = EducationLevel.Bachelors },
                    new Education { PersonId = sofia.Id, EducationLevel = EducationLevel.Bachelors }
                );

                // ===== MILITARY =====
                context.MilitaryServices.Add(
                    new MilitaryService
                    {
                        PersonId = carlos.Id,
                        MilitaryTypeId = 1,
                        StartDate = new DateTime(1985, 1, 1),
                        EndDate = new DateTime(1991, 1, 1)
                    });

                // ===== STORIES =====
                context.UserStories.AddRange(
                    new UserStory
                    {
                        PersonId = rafael.Id,
                        FamilyTreeId = familyTree.Id,
                        UserId = userId,
                        Story = "Rafael could fix anything."
                    },
                    new UserStory
                    {
                        PersonId = ana.Id,
                        FamilyTreeId = familyTree.Id,
                        UserId = userId,
                        Story = "Ana was always the one everyone called first."
                    }
                );

                await context.SaveChangesAsync();
            }
            else if (familyTree.FamilyName.Contains("Evenson"))
            {
                var marcy = new Person
                {
                    FirstName = "Marlene",
                    LastName = "Martin",
                    BirthDate = new DateTime(1932, 5, 10),
                    BirthPlace = "Ashland, WI",
                    Gender = GenderType.Female,
                    FamilyTreeId = familyTree.Id
                };

                var bud = new Person
                {
                    FirstName = "Edward",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1926, 4, 22),
                    BirthPlace = "Frederick, WI",
                    DeathDate = new DateTime(2013, 03, 28),
                    RestingPlace = "Superior, WI",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id
                };

                context.People.Add(marcy);
                await context.SaveChangesAsync();

                var brian = new Person
                {
                    FirstName = "Brian",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1958, 3, 14),
                    BirthPlace = "Ashland, WI",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = marcy.Id
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

                var alex = new Person
                {
                    FirstName = "Alex",
                    LastName = "Evenson",
                    BirthDate = new DateTime(1991, 7, 8),
                    BirthPlace = "Ashland, WI",
                    Gender = GenderType.Male,
                    FamilyTreeId = familyTree.Id,
                    Parent1Id = brian.Id,
                    Parent2Id = kathleen.Id
                };

                context.People.Add(kara);
                context.People.Add(alex);
                await context.SaveChangesAsync();
            }
        }
        }
    }
