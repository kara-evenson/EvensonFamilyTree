using EvensonFamilyTreeAppsDev.Models;
using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.Education
{
    public class EducationCreateViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;

        public EducationLevel EducationLevel { get; set; }

        public string? SchoolAttended { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
