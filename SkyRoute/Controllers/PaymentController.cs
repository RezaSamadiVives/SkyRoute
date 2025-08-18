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
    public class PaymentController(IShoppingcartService _shoppingcartService)  : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var cart = _shoppingcartService.GetShoppingCart(HttpContext.Session);
            if (cart == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var vm = new PaymentVM
            {
                Amount = cart.Total
            };

            return View(vm);
        }
        
    }
}