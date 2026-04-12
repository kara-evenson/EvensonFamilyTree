using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using EvensonFamilyTreeAppsDev.ViewModels.AuthorizedViewer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    [Authorize]
    public class AuthorizedViewerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AuthorizedViewerController(
            ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int familyTreeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var familyTree = await _context.FamilyTrees
                .FirstOrDefaultAsync(ft => ft.Id == familyTreeId && ft.OwnerId == userId);

            if (familyTree == null)
            {
                return NotFound();
            }

            var model = new AuthorizedViewerCreateViewModel
            {
                FamilyTreeId = familyTree.Id,
                FamilyTreeName = familyTree.FamilyName ?? "Family Tree"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorizedViewerCreateViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var familyTree = await _context.FamilyTrees
                .FirstOrDefaultAsync(ft => ft.Id == model.FamilyTreeId && ft.OwnerId == userId);

            if (familyTree == null)
            {
                return NotFound();
            }

            model.FamilyTreeName = familyTree.FamilyName ?? "Family Tree";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userToAuthorize = await _userManager.FindByEmailAsync(model.Email);

            if (userToAuthorize == null)
            {
                ModelState.AddModelError("Email", "No registered user was found with that email address.");
                return View(model);
            }

            if (userToAuthorize.Id == userId)
            {
                ModelState.AddModelError("Email", "You already own this family tree.");
                return View(model);
            }

            var alreadyAuthorized = await _context.AuthorizedViewers
                .AnyAsync(av => av.FamilyTreeId == model.FamilyTreeId && av.UserId == userToAuthorize.Id);

            if (alreadyAuthorized)
            {
                ModelState.AddModelError("Email", "That user is already an authorized viewer.");
                return View(model);
            }

            var authorizedViewer = new AuthorizedViewer
            {
                FamilyTreeId = model.FamilyTreeId,
                UserId = userToAuthorize.Id
            };

            _context.AuthorizedViewers.Add(authorizedViewer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "FamilyTree");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var authorizedViewer = await _context.AuthorizedViewers
                .Include(av => av.FamilyTree)
                .Include(av => av.User)
                .FirstOrDefaultAsync(av => av.Id == id && av.FamilyTree.OwnerId == userId);

            if (authorizedViewer == null)
            {
                return NotFound();
            }

            return View(authorizedViewer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var authorizedViewer = await _context.AuthorizedViewers
                .Include(av => av.FamilyTree)
                .FirstOrDefaultAsync(av => av.Id == id && av.FamilyTree.OwnerId == userId);

            if (authorizedViewer == null)
            {
                return NotFound();
            }

            _context.AuthorizedViewers.Remove(authorizedViewer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "FamilyTree");
        }
    }
}