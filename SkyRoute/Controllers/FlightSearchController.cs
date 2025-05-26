using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class FlightSearchController(IFlightSearchService flightSearchService, IMapper mapper) : Controller
    {
        private readonly IFlightSearchService _flightSearchService = flightSearchService;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> FlightSearchResults(int fromCityId, int toCityId,
            DateTime departureDate, DateTime? returnDate, bool isRetour, bool isBusiness, int adultPassengers, int? kidsPassengers)
        {
            try
            {
                var flightResults = await GetFlights(fromCityId, toCityId, departureDate, returnDate, isRetour, isBusiness,
                     adultPassengers, kidsPassengers);
                FlightSearchResultVM flightSearchResultVM = _mapper.Map<FlightSearchResultVM>(flightResults);

                flightSearchResultVM.FormModel = new FlightSearchFormVM
                {
                    DepartureCity = fromCityId,
                    DestinationCity = toCityId,
                    DepartureDate = departureDate,
                    ReturnDate = returnDate?.Date,
                    SelectedTripClass = isBusiness ? TripClass.Business : TripClass.Economy,
                    SelectedTripType = isRetour ? TripType.Retour : TripType.Enkel,
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

        private async Task<FlightSearchResult> GetFlights(int fromCityId, int toCityId,
            DateTime departureDate, DateTime? returnDate, bool isRetour, bool isBusiness, int adultPassengers, int? kidsPassengers)
        {
            return await _flightSearchService.SearchFlightsAsync(fromCityId, toCityId, departureDate, returnDate, isBusiness,
                 isRetour, adultPassengers, kidsPassengers);
        }

        [HttpPost]
        public IActionResult FlightSearchResults(FlightSearchFormVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("FlightSearchResults", new
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Select([FromBody] FlightSelectionVM selection)
        {
            if (selection.SegmentId == Guid.Empty)
                return BadRequest("Ongeldige segmentId");


            FlightSegmentGroup flightSegmentGroup = await _flightSearchService.GetAvailableFlights(selection.SegmentId, 
                selection.IsBusiness, selection.AdultPassengers, selection.KidsPassengers);

            if (flightSegmentGroup.Flights.Count == 0)
            {
                return Json(new { succes = false, selectedSegment = selection.SegmentId });
            }

            var shoppingCartVM = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") ?? new ShoppingCartVM();

            var existingItem = selection.IsRetour
                ? shoppingCartVM.RetourFlights?.SegmentId == selection.SegmentId
                : shoppingCartVM.OutboundFlights?.SegmentId == selection.SegmentId;


            if (!existingItem)
            {
                if (!selection.IsRetour)
                {
                    shoppingCartVM.OutboundFlights = new FlightSegmentSessionVM()
                    {
                        SegmentId = selection.SegmentId,
                        Flights = flightSegmentGroup.Flights.Select(f => f.Id).ToList(),
                        TotalDuration = flightSegmentGroup.TotalDuration,
                        TotalPrice = selection.IsBusiness ? flightSegmentGroup.Flights.Sum(f => f.PriceBusiness)
                        : flightSegmentGroup.Flights.Sum(f => f.PriceEconomy)
                    };

                }
                else
                {
                    shoppingCartVM.RetourFlights = new FlightSegmentSessionVM()
                    {
                        SegmentId = selection.SegmentId,
                        Flights = flightSegmentGroup.Flights.Select(f => f.Id).ToList(),
                        TotalDuration = flightSegmentGroup.TotalDuration,
                        TotalPrice = selection.IsBusiness ? flightSegmentGroup.Flights.Sum(f => f.PriceBusiness)
                        : flightSegmentGroup.Flights.Sum(f => f.PriceEconomy)
                    };

                }


            }


            HttpContext.Session.SetObject("ShoppingCart", shoppingCartVM);

            return Json(new { success = true, selectedSegment = selection.SegmentId });
        }

    }

}
