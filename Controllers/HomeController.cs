using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AgriEnergyConnect.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
