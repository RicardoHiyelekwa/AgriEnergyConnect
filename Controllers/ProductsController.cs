using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly Repository _repo;

        public ProductsController(DbConfig cfg) => _repo = new Repository(cfg);

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isEmployee = await _repo.IsInRoleAsync(uid, "Employee");

            if (isEmployee)
            {
                ViewBag.Farmers = await _repo.GetAllFarmersAsync();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name, string category, DateTime productionDate, int farmerId = 0)
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isFarmer = await _repo.IsInRoleAsync(uid, "Farmer");
            var isEmployee = await _repo.IsInRoleAsync(uid, "Employee");

            int targetFarmerId;

            if (isFarmer)
            {
                var farmer = await _repo.GetFarmerByUserIdAsync(uid);
                if (farmer == null)
                {
                    TempData["Err"] = "No farmer profile found for this user.";
                    return RedirectToAction("Add");
                }
                targetFarmerId = farmer.FarmerId;
            }
            else if (isEmployee)
            {
                if (farmerId == 0)
                {
                    TempData["Err"] = "Please select a farmer.";
                    return RedirectToAction("Add");
                }

                var farmer = await _repo.GetFarmerByIdAsync(farmerId);
                if (farmer == null)
                {
                    TempData["Err"] = "Selected farmer does not exist.";
                    return RedirectToAction("Add");
                }
                targetFarmerId = farmerId;
            }
            else
            {
                TempData["Err"] = "You are not authorized to add products.";
                return RedirectToAction("Add");
            }

            var product = new Product
            {
                FarmerId = targetFarmerId,
                Name = name,
                Category = category,
                ProductionDate = productionDate
            };

            await _repo.AddProductAsync(product);
            TempData["Msg"] = "Product added successfully.";
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List(int? farmerId, DateTime? from, DateTime? to, string? category)
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isFarmer = await _repo.IsInRoleAsync(uid, "Farmer");
            var isEmployee = await _repo.IsInRoleAsync(uid, "Employee");

            int targetFarmerId;

            if (isFarmer)
            {
                var farmer = await _repo.GetFarmerByUserIdAsync(uid);
                if (farmer == null)
                {
                    TempData["Err"] = "No farmer profile found for this user.";
                    return View(new List<Product>());
                }
                targetFarmerId = farmer.FarmerId;
            }
            else if (isEmployee)
            {
                if (!farmerId.HasValue)
                {
                    TempData["Err"] = "Please select a farmer.";
                    ViewBag.Farmers = await _repo.GetAllFarmersAsync();
                    return View(new List<Product>());
                }

                var farmer = await _repo.GetFarmerByIdAsync(farmerId.Value);
                if (farmer == null)
                {
                    TempData["Err"] = "Selected farmer does not exist.";
                    ViewBag.Farmers = await _repo.GetAllFarmersAsync();
                    return View(new List<Product>());
                }

                targetFarmerId = farmerId.Value;
                ViewBag.Farmers = await _repo.GetAllFarmersAsync();
            }
            else
            {
                TempData["Err"] = "You are not authorized to view products.";
                return View(new List<Product>());
            }

            var products = await _repo.GetProductsByFarmerAsync(targetFarmerId, from, to, category);

            ViewBag.FarmerId = targetFarmerId;
            ViewBag.From = from?.ToString("yyyy-MM-dd");
            ViewBag.To = to?.ToString("yyyy-MM-dd");
            ViewBag.Category = category ?? "";

            return View(products);
        }



    }
}
