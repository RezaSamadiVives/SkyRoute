using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services;

namespace SkyRoute.Controllers
{
    [Route("Session")]
    public class SessionController(IShoppingcartService _shoppingcartService) : Controller
    {
        [HttpPost("KeepAlive")]
        public IActionResult KeepAlive()
        {
            _shoppingcartService.SetSessionStart(HttpContext.Session,"LastPing");
            return Ok();
        }

        [HttpPost("Clear")]
        public IActionResult Clear()
        {
            _shoppingcartService.ClearSession(HttpContext.Session);
            return Ok();
        }
    }

}