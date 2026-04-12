using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.Occupation
{
    public class OccupationCreateViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;

        [Display(Name = "Job Title")]
        public string? Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}
