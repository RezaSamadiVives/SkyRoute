using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class AirlinesController(IService<Airline> _airlineService, IMapper _mapper) : Controller
    {
        // GET: Airlines
        public async Task<IActionResult> Index()
        {
            try
            {
                var airlines = await _airlineService.GetAllAsync();
                List<AirlineVM> airlinesVM = _mapper.Map<List<AirlineVM>>(airlines);
                return View(airlinesVM);
            }
            catch (Exception)
            {

                throw;
            }
        
        }

    }
}
