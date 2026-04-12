using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.Partnership
{
    public class PartnershipCreateViewModel
    {
        public int Person1Id { get; set; }

        public string Person1Name { get; set; } = string.Empty;

        [Display(Name = "Partner")]
        public int Person2Id { get; set; }

        [Display(Name = "Relationship Type")]
        public int? RelationshipTypeId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string? Notes { get; set; }
    }
}