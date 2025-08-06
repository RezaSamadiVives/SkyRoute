using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class FlightSearchController(
        IFlightSearchService _flightSearchService,
        IShoppingcartService _shoppingcartService,
         IFlightSearchHandler _flightSearchHandler,
        IMapper _mapper) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> FlightSearchResults(int? fromCityId, string? fromCity, int? toCityId, string? toCity,
                  DateTime? departureDate, DateTime? returnDate, bool? isRetour, bool? isBusiness, int? adultPassengers, int? kidsPassengers)
        {
            try
            {
                var vm = await _flightSearchHandler.HandleFlightSearchAsync(HttpContext,
                    fromCityId, fromCity, toCityId, toCity,
                    departureDate, returnDate, isRetour, isBusiness, adultPassengers, kidsPassengers);

                return View("FlightSearchResults", vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Er is iets misgegaan bij het zoeken van vluchten. Error: {ex.Message}");
                return View("FlightSearchResults", new FlightSearchResultVM());
            }
        }

        /*   [HttpGet]
          public async Task<IActionResult> FlightSearchResults(int fromCityId, string fromCity, int toCityId, string toCity,
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

                  var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);
                  shoppingCartVM.FlightSearchSessionVM = new FlightSearchSessionVM
                  {
                      ToCity = toCity,
                      ToCityId = toCityId,
                      FromCity = fromCity,
                      FromCityId = fromCityId,
                      OutboundFlightDate = departureDate,
                      ReturnFlightDate = returnDate?.Date,
                      AdultPassengers = adultPassengers,
                      KidsPassengers = kidsPassengers,
                      TripClass = isBusiness ? TripClass.Business : TripClass.Economy,
                      TripType = isRetour ? TripType.Retour : TripType.Enkel,
                  };

                  _shoppingcartService.SetShoppingObject(shoppingCartVM, HttpContext.Session);

                  return View("FlightSearchResults", flightSearchResultVM);
              }
              catch (Exception ex)
              {
                  ModelState.AddModelError("", $"Er is iets misgegaan bij het zoeken van vluchten. Error: {ex.Message}");
                  return RedirectToAction("Index", "Home");
              }

          }
   */
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
            try
            {
                var result = await _flightSearchHandler.GetSelectedFlightSegmentAsync(selection, HttpContext.Session);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        /*  [HttpPost]
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

               var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

               var existingItem = selection.IsRetour
                   ? shoppingCartVM.RetourFlights?.SegmentId == selection.SegmentId
                   : shoppingCartVM.OutboundFlights?.SegmentId == selection.SegmentId;


               if (!existingItem)
               {
                   if (!selection.IsRetour)
                   {
                       shoppingCartVM.OutboundFlights = CreateFlightSegmentSessionVm(selection, flightSegmentGroup, shoppingCartVM);

                   }
                   else
                   {
                       shoppingCartVM.RetourFlights = CreateFlightSegmentSessionVm(selection, flightSegmentGroup, shoppingCartVM);

                   }


               }

               _shoppingcartService.SetShoppingObject(shoppingCartVM, HttpContext.Session);

               return Json(new { success = true, selectedSegment = selection.SegmentId });
           }
    */
        private static FlightSegmentSessionVM CreateFlightSegmentSessionVm(FlightSelectionVM selection, FlightSegmentGroup flightSegmentGroup, ShoppingCartVM shoppingCartVM)
        {
            return new FlightSegmentSessionVM()
            {
                SegmentId = selection.SegmentId,
                Flights = [.. flightSegmentGroup.Flights.Select(f => f.Id)],
                TotalDuration = flightSegmentGroup.TotalDuration,
                TotalPrice = selection.IsBusiness ? flightSegmentGroup.Flights.Sum(f => f.PriceBusiness)
                : flightSegmentGroup.Flights.Sum(f => f.PriceEconomy)
            };
        }

    }

}
