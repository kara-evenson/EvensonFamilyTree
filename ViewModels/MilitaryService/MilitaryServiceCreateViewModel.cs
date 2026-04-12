using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.MilitaryService
{
    public class MilitaryServiceCreateViewModel
    {
        public int PersonId { get; set; }
        public string PersonName { get; set; } = string.Empty;

        public int MilitaryTypeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? Commendations { get; set; }

        public string? Notes { get; set; }
    }
}
