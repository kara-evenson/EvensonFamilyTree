namespace EvensonFamilyTreeAppsDev.Models
{
    public class MilitaryService
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int? MilitaryTypeId { get; set; }
        public MilitaryType? MilitaryType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Commendations { get; set; }
        public string? Notes { get; set; }
    }
}
