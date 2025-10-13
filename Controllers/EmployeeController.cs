using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? farmerId, string? category, DateTime? from, DateTime? to)
        {
            var farmers = await _context.Farmers.OrderBy(f => f.Name).ToListAsync();
            ViewData["Farmers"] = farmers;

            if (farmerId == null)
            {
                ViewData["Products"] = new List<Product>();
                return View();
            }

            var query = _context.Products.Where(p => p.FarmerId == farmerId);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (from.HasValue)
                query = query.Where(p => p.ProductionDate >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.ProductionDate <= to.Value);

            var products = await query.OrderByDescending(p => p.ProductionDate).ToListAsync();
            ViewData["Products"] = products;
            ViewData["SelectedFarmerId"] = farmerId;
            return View();
        }

        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int farmerId, string? category, DateTime? from, DateTime? to)
        {
            var query = _context.Products.Where(p => p.FarmerId == farmerId);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category.Contains(category));

            if (from.HasValue)
                query = query.Where(p => p.ProductionDate >= from);

            if (to.HasValue)
                query = query.Where(p => p.ProductionDate <= to);

            var products = await query.OrderByDescending(p => p.ProductionDate).ToListAsync();

            // Retorna apenas o HTML parcial da tabela
            return PartialView("_ProductsTablePartial", products);
        }


        [HttpPost]
        public async Task<IActionResult> AddFarmer(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Name and email required.");
                return View();
            }

            // create identity user for farmer with default password
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "A user with that email already exists.");
                return View();
            }

            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, "Farmer@1234");
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Could not create user: " + string.Join("; ", result.Errors.Select(e => e.Description)));
                return View();
            }
            await _userManager.AddToRoleAsync(user, "Farmer");

            var farmer = new Farmer { Name = name, Email = email, IdentityUserId = user.Id };
            _context.Farmers.Add(farmer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Farmer profile and user created. Default password: Farmer@1234";
            return RedirectToAction("Index");
        }
    }
}
