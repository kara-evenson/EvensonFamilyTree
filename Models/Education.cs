namespace EvensonFamilyTreeAppsDev.Models
{
    public class Education
    {
        public int Id { get; set; }
        public EducationLevel? EducationLevel { get; set; }
        public string? SchoolAttended { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
    }
}
