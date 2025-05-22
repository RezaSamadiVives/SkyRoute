using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class FlightSearchController(IFlightSearchService flightSearchService ,IMapper mapper) : Controller
    {
        private readonly IFlightSearchService _flightSearchService = flightSearchService;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> FlightSearchResults(int fromCityId, int toCityId, 
            DateTime departureDate, DateTime? returnDate, bool isRetour, bool isBusiness, int adultPassengers, int? kidsPassengers)
        {
            try
            {
                var flightResults = await _flightSearchService.SearchFlightsAsync(fromCityId,toCityId,departureDate,returnDate,isBusiness,
                    isRetour,adultPassengers,kidsPassengers);
                FlightSearchResultVM flightSearchResultVM = _mapper.Map<FlightSearchResultVM>(flightResults);

                flightSearchResultVM.FormModel = new FlightSearchFormVM
                {
                    DepartureCity = fromCityId,
                    DestinationCity = toCityId,
                    DepartureDate = departureDate,
                    ReturnDate = returnDate?.Date,
                    SelectedTripClass = isBusiness ? "Business" : "Economy",
                    SelectedTripType = isRetour ? "retour": "enkel",
                    AdultPassengers = adultPassengers,
                    KidsPassengers = kidsPassengers
                };

                return View("FlightSearchResults", flightSearchResultVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Er is iets misgegaan bij het zoeken van vluchten. Error: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
            
        }

        [HttpPost]
        public IActionResult FlightSearchResults(FlightSearchFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("FlightSearchResults",new
            {
                fromCityId = model.DepartureCity,
                toCityId = model.DestinationCity,
                departureDate = model.DepartureDate,
                returnDate = model.ReturnDate,
                isRetour = model.IsRetour,
                isBusiness = model.IsBusiness,
                adultPassengers = model.AdultPassengers,
                kidsPassengers = model.KidsPassengers
            });
        }

    }
    
}
