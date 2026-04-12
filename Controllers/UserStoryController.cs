using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.UserStory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class UserStoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserStoryController(ApplicationDbContext context)
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

            var canAccess = await UserCanAccessFamilyTreeAsync((int)person.FamilyTreeId);

            if (!canAccess)
            {
                return Forbid();
            }

            var model = new UserStoryCreateViewModel
            {
                PersonId = person.Id,
                FamilyTreeId = (int)person.FamilyTreeId,
                PersonName = $"{person.FirstName} {person.LastName}".Trim()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserStoryCreateViewModel model)
        {
            var person = await _context.People
                .Include(p => p.FamilyTree)
                .FirstOrDefaultAsync(p => p.Id == model.PersonId && p.FamilyTreeId == model.FamilyTreeId);

            if (person == null)
            {
                return NotFound();
            }

            var canAccess = await UserCanAccessFamilyTreeAsync(model.FamilyTreeId);

            if (!canAccess)
            {
                return Forbid();
            }

            model.PersonName = $"{person.FirstName} {person.LastName}".Trim();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var story = new UserStory
            {
                PersonId = model.PersonId,
                FamilyTreeId = model.FamilyTreeId,
                UserId = userId,
                Story = model.Story,
                CreatedOn = DateTime.UtcNow
            };

            _context.UserStories.Add(story);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Person", new { id = model.PersonId });
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