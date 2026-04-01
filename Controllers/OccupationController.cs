using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.Occupation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class OccupationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OccupationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int personId)
        {
            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                return NotFound();
            }

            var canEdit = await UserOwnsFamilyTreeAsync((int)person.FamilyTreeId);

            if (!canEdit)
            {
                return Forbid();
            }

            var model = new OccupationCreateViewModel
            {
                PersonId = person.Id,
                PersonName = $"{person.FirstName} {person.LastName}".Trim()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OccupationCreateViewModel model)
        {
            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == model.PersonId);

            if (person == null)
            {
                return NotFound();
            }

            var canEdit = await UserOwnsFamilyTreeAsync((int)person.FamilyTreeId);

            if (!canEdit)
            {
                return Forbid();
            }

            model.PersonName = $"{person.FirstName} {person.LastName}".Trim();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var occupation = new Occupation
            {
                PersonId = model.PersonId,
                Title = model.Title,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Notes = model.Notes
            };

            _context.Occupations.Add(occupation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = model.PersonId });
        }

        private async Task<bool> UserOwnsFamilyTreeAsync(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);
        }
    }
}