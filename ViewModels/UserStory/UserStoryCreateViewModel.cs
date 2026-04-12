using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.UserStory
{
    public class UserStoryCreateViewModel
    {
        public int PersonId { get; set; }
        public int FamilyTreeId { get; set; }

        public string PersonName { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        [Display(Name = "Story")]
        public string Story { get; set; } = string.Empty;
    }
}