using Microsoft.AspNetCore.Mvc;

namespace SkyRoute.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/NotFound")]
        public  IActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
