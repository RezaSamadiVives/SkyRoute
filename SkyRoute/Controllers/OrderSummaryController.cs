using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Models;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class OrderSummaryController(
        IShoppingcartService _shoppingcartService,
         IFlightSearchService _flightSearchService,
          IMealOptionViewModelBuilder _viewModelBuilder,
         IMapper _mapper) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new OrderSummaryVM();

            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (shoppingCartVM == null)
            {
                return View(model);
            }

            // vluchten ophalen
            var segmentIdOutbound = shoppingCartVM.OutboundFlights?.SegmentId;
            var segmentIdRetour = shoppingCartVM.RetourFlights?.SegmentId;
            var flightSearchSession = shoppingCartVM.FlightSearchSessionVM;
            if (segmentIdOutbound == null || flightSearchSession == null)
            {
                return View(model);
            }

            bool isBusiness = flightSearchSession.TripClass == TripClass.Business;
            model.TripClass = isBusiness ? TripClass.Business : TripClass.Economy;

            FlightSegmentGroup flightSegmentGroupOutbound = await _flightSearchService.GetAvailableFlights(
                segmentIdOutbound.Value,
                isBusiness,
                flightSearchSession.AdultPassengers,
                flightSearchSession.KidsPassengers);

            model.OutboundFlight = _mapper.Map<FlightSegmentGroupVM>(flightSegmentGroupOutbound);

            if (segmentIdRetour != null)
            {
                FlightSegmentGroup flightSegmentGroupRetour = await _flightSearchService.GetAvailableFlights(
                segmentIdRetour.Value,
                isBusiness,
                flightSearchSession.AdultPassengers,
                flightSearchSession.KidsPassengers);

                model.ReturnFlight = _mapper.Map<FlightSegmentGroupVM>(flightSegmentGroupRetour);
            }

            // passagiers ophalen
            model.PassengerListVM.Passengers = shoppingCartVM.Passengers;

            // maaltijden ophalen
            model.MealsSelection = await _viewModelBuilder.GetSelectedMealsAsync(HttpContext);

            model.IsConfirmed = shoppingCartVM.IsConfirmed;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmBooking()
        {
            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (shoppingCartVM == null)
            {
                return RedirectToAction("Index", "Home");
            }

            shoppingCartVM.IsConfirmed = true;
            _shoppingcartService.SetShoppingObject(shoppingCartVM, HttpContext.Session);

            
            return RedirectToAction("Index", "ShoppingCart");
        }

    }
}