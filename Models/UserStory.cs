namespace EvensonFamilyTreeAppsDev.Models
{
    public class UserStory
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public string? Story { get; set; }
    }
}
