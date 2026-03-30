namespace EvensonFamilyTreeAppsDev.Models
{
    public class MilitaryService
    {
        public int Id { get; set; }
        public int? MilitaryTypeId { get; set; }
        public MilitaryType? MilitaryType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
