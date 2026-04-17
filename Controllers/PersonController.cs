using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();

            var people = await _context.People
                .Include(p => p.FamilyTree)
                .Where(p => p.FamilyTree != null && p.FamilyTree.OwnerId == userId)
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();

            return View(people);
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .Include(p => p.Parent1)
                .Include(p => p.Parent2)
                .Include(p => p.Occupations)
                .Include(p => p.Educations)
                .Include(p => p.MilitaryServices)
                    .ThenInclude(ms => ms.MilitaryType)
                .Include(p => p.Stories)
                    .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                return NotFound();
            }

            var canAccess = await UserCanAccessFamilyTreeAsync((int)person.FamilyTreeId);

            if (!canAccess)
            {
                return Forbid();
            }

            var partnerships = await _context.Partnerships
                .Include(pt => pt.Person1)
                .Include(pt => pt.Person2)
                .Include(pt => pt.RelationshipType)
                .Where(pt => pt.Person1Id == person.Id || pt.Person2Id == person.Id)
                .OrderByDescending(pt => pt.StartDate)
                .ToListAsync();

            ViewBag.Partnerships = partnerships;

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IsOwner = person.FamilyTree.OwnerId == currentUserId;

            return View(person);
        }

        // GET: Person/Create
        public async Task<IActionResult> Create()
        {
            var familyTree = await GetCurrentUserFamilyTreeAsync();

            if (familyTree == null)
            {
                return RedirectToAction("Index", "FamilyTree");
            }

            var person = new Person
            {
                FamilyTreeId = familyTree.Id
            };

            await PopulateParentDropDowns();

            return View(person);
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            var familyTree = await GetCurrentUserFamilyTreeAsync();

            if (familyTree == null)
            {
                return RedirectToAction("Index", "FamilyTree");
            }

            person.FamilyTreeId = familyTree.Id;

            if (!await ParentIsValidForTree(person.Parent1Id, familyTree.Id))
            {
                ModelState.AddModelError("Parent1Id", "Parent 1 must belong to your family tree.");
            }

            if (!await ParentIsValidForTree(person.Parent2Id, familyTree.Id))
            {
                ModelState.AddModelError("Parent2Id", "Parent 2 must belong to your family tree.");
            }

            if (person.Parent1Id.HasValue && person.Parent1Id == person.Parent2Id)
            {
                ModelState.AddModelError("Parent2Id", "Parent 1 and Parent 2 cannot be the same person.");
            }

            if (ModelState.IsValid)
            {
                _context.People.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateParentDropDowns (person.Parent1Id, person.Parent2Id);

            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.FamilyTree != null &&
                    p.FamilyTree.OwnerId == userId);

            if (person == null)
            {
                return NotFound();
            }

            await PopulateParentDropDowns(person.Parent1Id, person.Parent2Id);

            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();

            var existingPerson = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.FamilyTree != null &&
                    p.FamilyTree.OwnerId == userId);

            if (existingPerson == null)
            {
                return NotFound();
            }

            if (!await FamilyTreeBelongsToCurrentUser(person.FamilyTreeId, userId))
            {
                ModelState.AddModelError("FamilyTreeId", "You can only assign this person to your own family tree.");
            }

            if (!await ParentIsValidForTree(person.Parent1Id, person.FamilyTreeId))
            {
                ModelState.AddModelError("Parent1Id", "Parent 1 must belong to your selected family tree.");
            }

            if (!await ParentIsValidForTree(person.Parent2Id, person.FamilyTreeId))
            {
                ModelState.AddModelError("Parent2Id", "Parent 2 must belong to your selected family tree.");
            }

            if (person.Parent1Id.HasValue && person.Parent1Id == person.Parent2Id)
            {
                ModelState.AddModelError("Parent2Id", "Parent 1 and Parent 2 cannot be the same person.");
            }

            if (ModelState.IsValid)
            {
                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;
                existingPerson.Prefix = person.Prefix;
                existingPerson.Suffix = person.Suffix;
                existingPerson.BirthDate = person.BirthDate;
                existingPerson.DeathDate = person.DeathDate;
                existingPerson.BirthPlace = person.BirthPlace;
                existingPerson.RestingPlace = person.RestingPlace;
                existingPerson.LifeDescription = person.LifeDescription;
                existingPerson.Gender = person.Gender;
                existingPerson.FamilyTreeId = person.FamilyTreeId;
                existingPerson.Parent1Id = person.Parent1Id;
                existingPerson.Parent2Id = person.Parent2Id;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateParentDropDowns(person.Parent1Id, person.Parent2Id);

            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.FamilyTree != null &&
                    p.FamilyTree.OwnerId == userId);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = GetCurrentUserId();

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.FamilyTree != null &&
                    p.FamilyTree.OwnerId == userId);

            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateParentDropDowns(
            int? selectedParent1 = null,
            int? selectedParent2 = null,
            int? currentPersonId = null)
        {
            var familyTree = await GetCurrentUserFamilyTreeAsync();

            if (familyTree == null)
            {
                ViewBag.Parent1Id = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Parent2Id = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.FamilyTreeName = "No family tree found";
                return;
            }

            ViewBag.FamilyTreeName = familyTree.FamilyName;

            var peopleQuery = _context.People
                .AsNoTracking()
                .Where(p => p.FamilyTreeId == familyTree.Id);

            if (currentPersonId.HasValue)
            {
                peopleQuery = peopleQuery.Where(p => p.Id != currentPersonId.Value);
            }

            var parentList = await peopleQuery
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new
                {
                    p.Id,
                    FullName = ((p.FirstName ?? "") + " " + (p.LastName ?? "")).Trim()
                })
                .ToListAsync();

            ViewBag.Parent1Id = new SelectList(parentList, "Id", "FullName", selectedParent1);
            ViewBag.Parent2Id = new SelectList(parentList, "Id", "FullName", selectedParent2);
        }

        private async Task<bool> PersonExists(int id)
        {
            return await _context.People.AnyAsync(e => e.Id == id);
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        private async Task<bool> FamilyTreeBelongsToCurrentUser(int? familyTreeId, string userId)
        {
            if (!familyTreeId.HasValue)
            {
                return false;
            }

            return await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId.Value && ft.OwnerId == userId);
        }

        private async Task<bool> ParentIsValidForTree(int? parentId, int? familyTreeId)
        {
            if (!parentId.HasValue)
            {
                return true;
            }

            return await _context.People
                .AnyAsync(p => p.Id == parentId.Value && p.FamilyTreeId == familyTreeId);
        }

        private async Task<FamilyTree?> GetCurrentUserFamilyTreeAsync()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            return await _context.FamilyTrees
                .AsNoTracking()
                .FirstOrDefaultAsync(ft => ft.OwnerId == userId);
        }

        private async Task<bool> UserCanAccessFamilyTreeAsync(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var isOwner = await _context.FamilyTrees
                .AnyAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);

            if (isOwner)
            {
                return true;
            }

            var isAuthorizedViewer = await _context.AuthorizedViewers
                .AnyAsync(av => av.FamilyTreeId == familyTreeId && av.UserId == userId);

            return isAuthorizedViewer;
        }


    }
}


