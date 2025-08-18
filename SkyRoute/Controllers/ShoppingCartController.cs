using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class ShoppingCartController(IShoppingcartService _shoppingcartService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            ShoppingCartVM? cartList =
              _shoppingcartService.GetShoppingCart(HttpContext.Session);

            return View(cartList);
        }

        [HttpPost]
        public IActionResult Buy()
        {
            return RedirectToAction("Index","Payment");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveSegment(string segment)
        {
            var cart = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (segment == "outbound")
                cart.OutboundFlights = null;

            if (segment == "retour")
                cart.RetourFlights = null;

            _shoppingcartService.SetShoppingObject(cart,HttpContext.Session);

            return Json(new { success = true });
        }

    }

}