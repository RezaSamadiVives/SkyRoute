using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class MealOptionController(
        IShoppingcartService _shoppingcartService,
        IMealOptionViewModelBuilder _viewModelBuilder,
        IMealOptionSelectionService _mealoptionSelectionService
        ) : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return RedirectToAction("Index", "Home");
            }

            var vm = await _viewModelBuilder.BuildMealSelectionFormAsync(HttpContext);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMealOption([FromBody] MealSelectionPassengerVM selection)
        {
            var (success, message) = await _mealoptionSelectionService.SaveMealOptionAsync(selection, HttpContext);
            return Json(new { success, message });
        }

    }
}