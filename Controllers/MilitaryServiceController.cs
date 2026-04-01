using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.MilitaryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    public class MilitaryServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MilitaryServiceController(ApplicationDbContext context)
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

            var model = new MilitaryServiceCreateViewModel
            {
                PersonId = person.Id,
                PersonName = $"{person.FirstName} {person.LastName}".Trim()
            };

            ViewBag.MilitaryTypeId = new SelectList(
                _context.MilitaryTypes
                    .AsNoTracking()
                    .OrderBy(mt => mt.MilitaryBranch)
                    .ToList(),
                "Id",
                "MilitaryBranch");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MilitaryServiceCreateViewModel model)
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

            var militaryService = new MilitaryService
            {
                PersonId = model.PersonId,
                MilitaryTypeId = model.MilitaryTypeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Commendations = model.Commendations,
                Notes = model.Notes
            };

            _context.MilitaryServices.Add(militaryService);
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