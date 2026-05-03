using System.Diagnostics;
using System.Security.Claims;
using EvensonFamilyTreeAppsDev.Data;
using EvensonFamilyTreeAppsDev.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvensonFamilyTreeAppsDev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var people = await _context.People
                .Include(p => p.Parent1)
                .Include(p => p.Parent2)
                .Include(p => p.FamilyTree)
                .Where(p => p.FamilyTree != null && p.FamilyTree.OwnerId == userId)
                .ToListAsync();

            var partnerships = await _context.Partnerships
                .Include(p => p.Person1)
                    .ThenInclude(p => p.FamilyTree)
                .Include(p => p.Person2)
                .Include(p => p.RelationshipType)
                .Where(p => p.Person1.FamilyTree.OwnerId == userId)
                .ToListAsync();

            ViewBag.Partnerships = partnerships;

            var generationMap = people.ToDictionary(
                p => p.Id,
                p => GetGenerationLevel(p, people)
            );

                        foreach (var partnership in partnerships)
                        {
                            if (generationMap.ContainsKey(partnership.Person1Id) &&
                                generationMap.ContainsKey(partnership.Person2Id))
                            {
                                var generation = Math.Max(
                                    generationMap[partnership.Person1Id],
                                    generationMap[partnership.Person2Id]
                                );

                                generationMap[partnership.Person1Id] = generation;
                                generationMap[partnership.Person2Id] = generation;
                            }
                        }

                        var generations = people
                            .GroupBy(p => generationMap[p.Id])
                            .OrderBy(g => g.Key)
                            .ToDictionary(g => g.Key, g => g.OrderBy(p => p.BirthDate).ToList());

            return View(generations);
        }

        private int GetGenerationLevel(Person person, List<Person> allPeople)
        {
            if (person.Parent1Id == null && person.Parent2Id == null)
            {
                return 1;
            }

            var parents = allPeople
                .Where(p => p.Id == person.Parent1Id || p.Id == person.Parent2Id)
                .ToList();

            if (!parents.Any())
            {
                return 1;
            }

            return parents.Max(p => GetGenerationLevel(p, allPeople)) + 1;
        }
    }
}