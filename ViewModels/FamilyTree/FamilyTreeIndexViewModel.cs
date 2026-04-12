using EvensonFamilyTreeAppsDev.Models;

namespace EvensonFamilyTreeAppsDev.ViewModels.FamilyTree
{
    public class FamilyTreeIndexViewModel
    {
        public List<Models.FamilyTree> OwnedTrees { get; set; } = new();
        public List<Models.FamilyTree> SharedTrees { get; set; } = new();
    }
}