using System.ComponentModel.DataAnnotations;

namespace EvensonFamilyTreeAppsDev.Models
{
    public class UserStory
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        public int FamilyTreeId { get; set; }
        public FamilyTree FamilyTree { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Story { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}