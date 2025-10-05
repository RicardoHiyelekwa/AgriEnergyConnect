using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AgriEnergyConnect.Models;
using System.Security.Claims;

namespace AgriEnergyConnect.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string? role)
        {
            ViewData["ExpectedRole"] = role;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? expectedRole, string? returnUrl)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                ViewData["ExpectedRole"] = expectedRole;
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                ViewData["ExpectedRole"] = expectedRole;
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                ViewData["ExpectedRole"] = expectedRole;
                return View();
            }

            if (!string.IsNullOrEmpty(expectedRole))
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(expectedRole))
                {
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty, $"This account is not in the required role: {expectedRole}.");
                    ViewData["ExpectedRole"] = expectedRole;
                    return View();
                }
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // Redirect based on role
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("Employee")) return RedirectToAction("Index", "Employee");
            if (userRoles.Contains("Farmer")) return RedirectToAction("Index", "FarmerProducts");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
