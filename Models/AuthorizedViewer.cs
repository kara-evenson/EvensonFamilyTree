namespace EvensonFamilyTreeAppsDev.Models
{
    public class AuthorizedViewer
    {
        public int Id { get; set; }

        public int FamilyTreeId { get; set; }
        public FamilyTree FamilyTree { get; set; } = null!;

        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
