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

            if (person == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownsTree = await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == person.FamilyTreeId && ft.OwnerId == userId);

            if (!ownsTree)
            {
                return Forbid();
            }

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

            if (person1 == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ownsTree = await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == person1.FamilyTreeId && ft.OwnerId == userId);

            if (!ownsTree)
            {
                return Forbid();
            }

            model.Person1Name = $"{person1.FirstName} {person1.LastName}".Trim();

            var person2 = await _context.People
                .FirstOrDefaultAsync(p => p.Id == model.Person2Id);

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
    }
}