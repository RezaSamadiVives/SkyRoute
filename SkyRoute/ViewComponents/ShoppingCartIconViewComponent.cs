using Microsoft.AspNetCore.Mvc;
using SkyRoute.Extensions;
using SkyRoute.ViewModels;

namespace SkyRoute.ViewComponents
{
    public class ShoppingCartIconViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            bool isCartEmpty = cart == null || cart.OutboundFlights?.Flights.Count == 0;

            string iconName = isCartEmpty ? "shoppingcart_klein.png" : "shoppingcart_full.png";

            ViewData["CartIcon"] = iconName;

           return await Task.FromResult(View("ShoppingCartIcon"));

        }
    }

}