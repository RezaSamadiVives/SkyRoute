using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkyRoute.Models;
using SkyRoute.ViewModels;
using System.Diagnostics;

namespace SkyRoute.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new HomeVM
            {
                Cities =
                [
                    new SelectListItem { Value = "NYC", Text = "New York" },
                    new SelectListItem { Value = "LON", Text = "Londen" },
                    new SelectListItem { Value = "TYO", Text = "Tokio" },
                    new SelectListItem { Value = "SYD", Text = "Sydney" },
                    new SelectListItem { Value = "DXB", Text = "Dubai" },
                    new SelectListItem { Value = "CPT", Text = "Kaapstad" },
                    new SelectListItem { Value = "SIN", Text = "Singapore" }
                ]
            };
            
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
