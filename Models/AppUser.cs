using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EvensonFamilyTreeAppsDev.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FamilyTree> OwnedFamilyTrees { get; set; } = new List<FamilyTree>();
    }
}