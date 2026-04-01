namespace EvensonFamilyTreeAppsDev.Models
{
    public class FamilyTree
    {
        public int Id { get; set; }
        public string? FamilyName { get; set; }

        public string? OwnerId { get; set; }
        public AppUser? Owner { get; set; }

        public ICollection<Person> Members { get; set; } = new List<Person>();
        public ICollection<AuthorizedViewer> AuthorizedViewers { get; set; } = new List<AuthorizedViewer>();
        public ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();
    }
}