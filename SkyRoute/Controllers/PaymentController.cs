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
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        
    }
}