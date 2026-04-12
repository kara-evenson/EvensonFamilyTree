using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.ViewModels.AuthorizedViewer
{
    public class AuthorizedViewerCreateViewModel
    {
        public int FamilyTreeId { get; set; }

        public string FamilyTreeName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Viewer Email")]
        public string Email { get; set; } = string.Empty;
    }
}