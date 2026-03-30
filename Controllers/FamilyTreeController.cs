using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    public class FamilyTreeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamilyTreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /FamilyTree
        public async Task<IActionResult> Index()
        {
            var familyTreeCount = await _context.FamilyTrees.CountAsync();
            var peopleCount = await _context.People.CountAsync();

            ViewBag.FamilyTreeCount = familyTreeCount;
            ViewBag.PeopleCount = peopleCount;

            var familyTrees = await _context.FamilyTrees
                .Include(ft => ft.Members)
                    .ThenInclude(p => p.Parent1)
                .Include(ft => ft.Members)
                    .ThenInclude(p => p.Parent2)
                .OrderBy(ft => ft.FamilyName)
                .ToListAsync();

            return View(familyTrees);
        }
    }
}