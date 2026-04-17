using System.Security.Claims;
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
    [Authorize]
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
                await PopulateMilitaryTypesAsync(model.MilitaryTypeId);
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

        public async Task<IActionResult> Edit(int id)
        {
            var militaryService = await _context.MilitaryServices
                .Include(ms => ms.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(ms => ms.Id == id);

            if (militaryService == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)militaryService.Person.FamilyTreeId)) return Forbid();

            await PopulateMilitaryTypesAsync(militaryService.MilitaryTypeId);

            var model = new MilitaryServiceCreateViewModel
            {
                PersonId = militaryService.PersonId,
                PersonName = $"{militaryService.Person.FirstName} {militaryService.Person.LastName}".Trim(),
                MilitaryTypeId = (int)militaryService.MilitaryTypeId,
                StartDate = militaryService.StartDate,
                EndDate = militaryService.EndDate,
                Commendations = militaryService.Commendations,
                Notes = militaryService.Notes
            };

            ViewBag.MilitaryServiceId = militaryService.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MilitaryServiceCreateViewModel model)
        {
            var militaryService = await _context.MilitaryServices
                .Include(ms => ms.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(ms => ms.Id == id);

            if (militaryService == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)militaryService.Person.FamilyTreeId)) return Forbid();

            model.PersonName = $"{militaryService.Person.FirstName} {militaryService.Person.LastName}".Trim();

            if (!ModelState.IsValid)
            {
                await PopulateMilitaryTypesAsync(model.MilitaryTypeId);
                ViewBag.MilitaryServiceId = militaryService.Id;
                return View(model);
            }

            militaryService.MilitaryTypeId = model.MilitaryTypeId;
            militaryService.StartDate = model.StartDate;
            militaryService.EndDate = model.EndDate;
            militaryService.Commendations = model.Commendations;
            militaryService.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = militaryService.PersonId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var militaryService = await _context.MilitaryServices
                .Include(ms => ms.Person)
                    .ThenInclude(p => p.FamilyTree)
                .Include(ms => ms.MilitaryType)
                .FirstOrDefaultAsync(ms => ms.Id == id);

            if (militaryService == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)militaryService.Person.FamilyTreeId)) return Forbid();

            return View(militaryService);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var militaryService = await _context.MilitaryServices
                .Include(ms => ms.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(ms => ms.Id == id);

            if (militaryService == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)militaryService.Person.FamilyTreeId)) return Forbid();

            var personId = militaryService.PersonId;

            _context.MilitaryServices.Remove(militaryService);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = personId });
        }

        private async Task PopulateMilitaryTypesAsync(int? selectedId = null)
        {
            var militaryTypes = await _context.MilitaryTypes
                .AsNoTracking()
                .OrderBy(mt => mt.MilitaryBranch)
                .ToListAsync();

            ViewBag.MilitaryTypeId = new SelectList(militaryTypes, "Id", "MilitaryBranch", selectedId);
        }

        private async Task<bool> UserOwnsFamilyTreeAsync(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);
        }
    }
}