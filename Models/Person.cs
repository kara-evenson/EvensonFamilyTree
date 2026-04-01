using Microsoft.AspNetCore.Identity;

namespace EvensonFamilyTreeAppsDev.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        public string? BirthPlace { get; set; }
        public string? RestingPlace { get; set; }

        public string? LifeDescription { get; set; }

        public GenderType? Gender { get; set; }

        public int? FamilyTreeId { get; set; }
        public FamilyTree? FamilyTree { get; set; }

        // Parent Relationships
        public int? Parent1Id { get; set; }
        public Person? Parent1 { get; set; }

        public int? Parent2Id { get; set; }
        public Person? Parent2 { get; set; }

        public ICollection<Person> ChildrenFromParent1 { get; set; } = new List<Person>();
        public ICollection<Person> ChildrenFromParent2 { get; set; } = new List<Person>();

        public ICollection<UserStory> Stories { get; set; } = new List<UserStory>();
        public ICollection<Occupation> Occupations { get; set; } = new List<Occupation>();
        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<MilitaryService> MilitaryServices { get; set; } = new List<MilitaryService>();
    }
}
