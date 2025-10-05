using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerProductsController : Controller
    {
        private readonly AppDbContext _context;

        public FarmerProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.IdentityUserId == userId);
            if (farmer == null) return View(new List<Product>());

            var products = await _context.Products
                .Where(p => p.FarmerId == farmer.Id)
                .OrderByDescending(p => p.ProductionDate)
                .ToListAsync();

            ViewData["FarmerName"] = farmer.Name;
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int farmerId, string name, string category, DateTime productionDate)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category))
            {
                TempData["Error"] = "Name and category are required.";
                return RedirectToAction("Index");
            }

            var prod = new Product
            {
                Name = name,
                Category = category,
                ProductionDate = productionDate,
                FarmerId = farmerId
            };
            _context.Products.Add(prod);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product added.";
            return RedirectToAction("Index");
        }
    }
}
