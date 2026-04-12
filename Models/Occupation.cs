namespace EvensonFamilyTreeAppsDev.Models
{
    public class Occupation
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

    }
}