using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.Partnership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class PartnershipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int personId)
        {
            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)person.FamilyTreeId)) return Forbid();

            var model = new PartnershipCreateViewModel
            {
                Person1Id = person.Id,
                Person1Name = $"{person.FirstName} {person.LastName}".Trim()
            };

            await PopulateDropDowns((int)person.FamilyTreeId, person.Id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartnershipCreateViewModel model)
        {
            var person1 = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == model.Person1Id);

            if (person1 == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)person1.FamilyTreeId)) return Forbid();

            model.Person1Name = $"{person1.FirstName} {person1.LastName}".Trim();

            var person2 = await _context.People.FirstOrDefaultAsync(p => p.Id == model.Person2Id);

            if (person2 == null || person2.FamilyTreeId != person1.FamilyTreeId)
            {
                ModelState.AddModelError("Person2Id", "Selected partner must belong to the same family tree.");
            }

            if (model.Person1Id == model.Person2Id)
            {
                ModelState.AddModelError("Person2Id", "A person cannot be partnered with themselves.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropDowns((int)person1.FamilyTreeId, person1.Id, model.Person2Id, model.RelationshipTypeId);
                return View(model);
            }

            var partnership = new Partnership
            {
                Person1Id = model.Person1Id,
                Person2Id = model.Person2Id,
                RelationshipTypeId = model.RelationshipTypeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Notes = model.Notes
            };

            _context.Partnerships.Add(partnership);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = model.Person1Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var partnership = await _context.Partnerships
                .Include(p => p.Person1)
                    .ThenInclude(p => p.FamilyTree)
                .Include(p => p.Person2)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partnership == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)partnership.Person1.FamilyTreeId)) return Forbid();

            var model = new PartnershipCreateViewModel
            {
                Person1Id = partnership.Person1Id,
                Person1Name = $"{partnership.Person1.FirstName} {partnership.Person1.LastName}".Trim(),
                Person2Id = partnership.Person2Id,
                RelationshipTypeId = partnership.RelationshipTypeId,
                StartDate = partnership.StartDate,
                EndDate = partnership.EndDate,
                Notes = partnership.Notes
            };

            await PopulateDropDowns((int)partnership.Person1.FamilyTreeId, partnership.Person1Id, partnership.Person2Id, partnership.RelationshipTypeId);
            ViewBag.PartnershipId = partnership.Id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartnershipCreateViewModel model)
        {
            var partnership = await _context.Partnerships
                .Include(p => p.Person1)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partnership == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)partnership.Person1.FamilyTreeId)) return Forbid();

            model.Person1Name = $"{partnership.Person1.FirstName} {partnership.Person1.LastName}".Trim();

            var person2 = await _context.People.FirstOrDefaultAsync(p => p.Id == model.Person2Id);

            if (person2 == null || person2.FamilyTreeId != partnership.Person1.FamilyTreeId)
            {
                ModelState.AddModelError("Person2Id", "Selected partner must belong to the same family tree.");
            }

            if (model.Person1Id == model.Person2Id)
            {
                ModelState.AddModelError("Person2Id", "A person cannot be partnered with themselves.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropDowns((int)partnership.Person1.FamilyTreeId, partnership.Person1Id, model.Person2Id, model.RelationshipTypeId);
                ViewBag.PartnershipId = partnership.Id;
                return View(model);
            }

            partnership.Person2Id = model.Person2Id;
            partnership.RelationshipTypeId = model.RelationshipTypeId;
            partnership.StartDate = model.StartDate;
            partnership.EndDate = model.EndDate;
            partnership.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = partnership.Person1Id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var partnership = await _context.Partnerships
                .Include(p => p.Person1)
                    .ThenInclude(p => p.FamilyTree)
                .Include(p => p.Person2)
                .Include(p => p.RelationshipType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partnership == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)partnership.Person1.FamilyTreeId)) return Forbid();

            return View(partnership);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnership = await _context.Partnerships
                .Include(p => p.Person1)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (partnership == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)partnership.Person1.FamilyTreeId)) return Forbid();

            var personId = partnership.Person1Id;

            _context.Partnerships.Remove(partnership);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = personId });
        }

        private async Task PopulateDropDowns(
            int familyTreeId,
            int currentPersonId,
            int? selectedPerson2Id = null,
            int? selectedRelationshipTypeId = null)
        {
            var possiblePartners = await _context.People
                .AsNoTracking()
                .Where(p => p.FamilyTreeId == familyTreeId && p.Id != currentPersonId)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new
                {
                    p.Id,
                    FullName = ((p.FirstName ?? "") + " " + (p.LastName ?? "")).Trim()
                })
                .ToListAsync();

            ViewBag.Person2Id = new SelectList(possiblePartners, "Id", "FullName", selectedPerson2Id);

            var relationshipTypes = await _context.RelationshipTypes
                .AsNoTracking()
                .OrderBy(rt => rt.Name)
                .ToListAsync();

            ViewBag.RelationshipTypeId = new SelectList(relationshipTypes, "Id", "Name", selectedRelationshipTypeId);
        }

        private async Task<bool> UserOwnsFamilyTreeAsync(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);
        }
    }
}