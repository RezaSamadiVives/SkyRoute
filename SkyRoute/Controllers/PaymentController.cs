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
    public class PaymentController(IShoppingcartService _shoppingcartService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var cart = _shoppingcartService.GetShoppingCart(HttpContext.Session);
            if (cart == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var amount = GetCurrentAmount();
            if (amount == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var vm = new PaymentVM
            {
                Amount = amount
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessPayment(PaymentVM paymentVM)
        {
            paymentVM.Amount = GetCurrentAmount();

            if (!ModelState.IsValid)
            {
                return View("Index", paymentVM);
            }

            var cart = _shoppingcartService.GetShoppingCart(HttpContext.Session);
            if (cart == null || cart.Passengers.Count == 0 || cart.OutboundFlights == null)
            {
                ModelState.AddModelError("", "Geen geldige boeking gevonden.");
                return View("Index", paymentVM);
            }

            cart.PaymentDetail = paymentVM;
            _shoppingcartService.SetShoppingObject(cart, HttpContext.Session);

            return RedirectToAction("Index", "TicketBooking");
        }

        private decimal GetCurrentAmount()
        {
            var cart = _shoppingcartService.GetShoppingCart(HttpContext.Session);
            return cart?.Total ?? 0;
        }

    }
}