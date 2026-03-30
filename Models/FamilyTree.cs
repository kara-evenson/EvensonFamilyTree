namespace EvensonFamilyTreeAppsDev.Models
{
    public class FamilyTree
    {
        public int Id { get; set; }
        public string? FamilyName { get; set; }

        public int? OwnerId { get; set; }
        public AppUser? Owner { get; set; }

        public ICollection<Person> Members { get; set; } = new List<Person>();
        public ICollection<AuthorizedViewer> AuthorizedViewers { get; set; } = new List<AuthorizedViewer>();
    }
}
