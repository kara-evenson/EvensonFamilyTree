using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
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
            var people = await _context.People
                .Include(p => p.FamilyTree)
                .ToListAsync();

            return View(people);
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .Include(p => p.Parent1)
                .Include(p => p.Parent2)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return NotFound();

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(person.FamilyTreeId, person.Parent1Id, person.Parent2Id);
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var person = await _context.People.FindAsync(id);
            if (person == null) return NotFound();

            PopulateDropDowns(person.FamilyTreeId, person.Parent1Id, person.Parent2Id, person.Id);
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Person person)
        {
            if (id != person.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateDropDowns(person.FamilyTreeId, person.Parent1Id, person.Parent2Id, person.Id);
            return View(person);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return NotFound();

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);

            if (person != null)
            {
                _context.People.Remove(person);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void PopulateDropDowns(
            int? selectedFamilyTree = null,
            int? selectedParent1 = null,
            int? selectedParent2 = null,
            int? currentPersonId = null)
        {
            ViewBag.FamilyTreeId = new SelectList(
                _context.FamilyTrees
                    .AsNoTracking()
                    .OrderBy(ft => ft.FamilyName)
                    .ToList(),
                "Id",
                "FamilyName",
                selectedFamilyTree);

            var peopleQuery = _context.People
                .AsNoTracking()
                .Where(p => p.FamilyTreeId == selectedFamilyTree);

            if (currentPersonId.HasValue)
            {
                peopleQuery = peopleQuery.Where(p => p.Id != currentPersonId.Value);
            }

            var parentList = peopleQuery
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Select(p => new
                {
                    p.Id,
                    FullName = (p.FirstName ?? "") + " " + (p.LastName ?? "")
                })
                .ToList();

            ViewBag.Parent1Id = new SelectList(parentList, "Id", "FullName", selectedParent1);
            ViewBag.Parent2Id = new SelectList(parentList, "Id", "FullName", selectedParent2);
        }

        private async Task<bool> PersonExists(int id)
        {
            return await _context.People.AnyAsync(e => e.Id == id);
        }
    }
}


