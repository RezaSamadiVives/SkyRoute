using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Extensions;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class MealOptionController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}