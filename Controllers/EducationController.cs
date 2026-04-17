using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class EducationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EducationController(ApplicationDbContext context)
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

            var model = new EducationCreateViewModel
            {
                PersonId = person.Id,
                PersonName = $"{person.FirstName} {person.LastName}".Trim()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EducationCreateViewModel model)
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

            var education = new Education
            {
                PersonId = model.PersonId,
                EducationLevel = model.EducationLevel,
                SchoolAttended = model.SchoolAttended,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Notes = model.Notes
            };

            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = model.PersonId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var education = await _context.Educations
                .Include(e => e.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)education.Person.FamilyTreeId)) return Forbid();

            var model = new EducationCreateViewModel
            {
                PersonId = education.PersonId,
                PersonName = $"{education.Person.FirstName} {education.Person.LastName}".Trim(),
                EducationLevel = (EducationLevel)education.EducationLevel,
                SchoolAttended = education.SchoolAttended,
                StartDate = education.StartDate,
                EndDate = education.EndDate,
                Notes = education.Notes
            };

            ViewBag.EducationId = education.Id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EducationCreateViewModel model)
        {
            var education = await _context.Educations
                .Include(e => e.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)education.Person.FamilyTreeId)) return Forbid();

            model.PersonName = $"{education.Person.FirstName} {education.Person.LastName}".Trim();

            if (!ModelState.IsValid)
            {
                ViewBag.EducationId = education.Id;
                return View(model);
            }

            education.EducationLevel = model.EducationLevel;
            education.SchoolAttended = model.SchoolAttended;
            education.StartDate = model.StartDate;
            education.EndDate = model.EndDate;
            education.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = education.PersonId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var education = await _context.Educations
                .Include(e => e.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)education.Person.FamilyTreeId)) return Forbid();

            return View(education);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var education = await _context.Educations
                .Include(e => e.Person)
                    .ThenInclude(p => p.FamilyTree)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (education == null) return NotFound();
            if (!await UserOwnsFamilyTreeAsync((int)education.Person.FamilyTreeId)) return Forbid();

            var personId = education.PersonId;

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = personId });
        }

        private async Task<bool> UserOwnsFamilyTreeAsync(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);
        }
    }
}