using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    public class FlightSearchController(IFlightSearchHandler _flightSearchHandler) : Controller
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

    }

}
