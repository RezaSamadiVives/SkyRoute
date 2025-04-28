using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class AirlinesController(IService<Airline> airlineService, IMapper mapper) : Controller
    {
        private readonly IService<Airline> airlineService = airlineService;
        private readonly IMapper _mapper = mapper;

        // GET: Airlines
        public async Task<IActionResult> Index()
        {
            try
            {
                var airlines = await airlineService.GetAllAsync();
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
