using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.ViewModels.FamilyTree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class FamilyTreeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FamilyTreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var ownedTrees = await _context.FamilyTrees
                .Where(ft => ft.OwnerId == userId)
                .Include(ft => ft.Members)
                .Include(ft => ft.AuthorizedViewers)
                    .ThenInclude(av => av.User)
                .OrderBy(ft => ft.FamilyName)
                .ToListAsync();

            var sharedTrees = await _context.FamilyTrees
                .Where(ft => ft.OwnerId != userId && ft.AuthorizedViewers.Any(av => av.UserId == userId))
                .Include(ft => ft.Members)
                .Include(ft => ft.Owner)
                .OrderBy(ft => ft.FamilyName)
                .ToListAsync();

            var model = new FamilyTreeIndexViewModel
            {
                OwnedTrees = ownedTrees,
                SharedTrees = sharedTrees
            };

            return View(model);
        }
    }
}