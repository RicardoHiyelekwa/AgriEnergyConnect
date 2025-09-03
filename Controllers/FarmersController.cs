using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using AgriEnergyConnect.Data;
using AgriEnergyConnect.Models;

namespace AgriEnergyConnect.Controllers
{
    [Authorize]
    public class FarmersController : Controller
    {
        private readonly Repository _repo;

        public FarmersController(DbConfig cfg) => _repo = new Repository(cfg);

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(Farmer model)
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var isEmployee = await _repo.IsInRoleAsync(uid, "Employee");
            var existingFarmer = await _repo.GetFarmerByUserIdAsync(uid);

            if (existingFarmer != null)
            {
                TempData["Err"] = "You already have a farmer profile.";
                return RedirectToAction("Add");
            }

            if (!isEmployee)
            {
                // Farmer criando seu próprio perfil
                await _repo.AddFarmerAsync(model, uid);
                TempData["Msg"] = "Your farmer profile has been created successfully.";
                return RedirectToAction("Add");
            }

            // Employee criando outro farmer
            await _repo.AddFarmerAsync(model);
            TempData["Msg"] = "Farmer added successfully by Employee.";
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var farmers = await _repo.GetAllFarmersAsync();
            return View(farmers); // ok, model é IEnumerable<Farmer>
        }


    }


}
