using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Extensions;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class TicketBookingController(
        IShoppingcartService _shoppingcartService,
        IBookingService _bookingService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var shoppingCartVM = _shoppingcartService.GetShoppingCart(HttpContext.Session);

            if (shoppingCartVM == null || shoppingCartVM.OutboundFlights == null || shoppingCartVM.OutboundFlights?.Flights.Count == 0)
            {
                ModelState.AddModelError("", "Er zijn geen vluchtgegevens beschikbaar. Verzoek eerst een vlucht te kiezen.");
                return View();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Er is geen ingelogde gebruiker. Log in en probeer het opnieuw.");
                return View();
            }

            if (shoppingCartVM.OutboundFlights != null && shoppingCartVM.FlightSearchSessionVM != null)
            {

                try
                {


                    var bookingRequest = new BookingRequest
                    {
                        UserId = userId,
                        SegmentIdOutbound = shoppingCartVM.OutboundFlights.SegmentId,
                        IsBusiness = shoppingCartVM.FlightSearchSessionVM.TripClass == TripClass.Business,
                        PassengersCount =
                        shoppingCartVM.FlightSearchSessionVM.AdultPassengers +
                        (shoppingCartVM.FlightSearchSessionVM.KidsPassengers ?? 0)
                    };

                    if (shoppingCartVM.RetourFlights != null)
                    {
                        bookingRequest.SegmentIdRetour = shoppingCartVM.RetourFlights.SegmentId;
                    }

                    foreach (var passenger in shoppingCartVM.Passengers)
                    {
                        var passengerFlightMeals = new PassengerFlightMeals
                        {
                            FirstName = passenger.FirstName,
                            MiddelName = passenger.MiddelName,
                            LastName = passenger.LastName,
                            Birthday = passenger.Birthday,
                            IsFellowPassenger = passenger.IsFellowPassenger
                        };

                        var mealchoices = shoppingCartVM.MealChoicePassengerSessions
                        .Where(x => x.PassengerId == passenger.Id).ToList();

                        if (mealchoices.Count == 0)
                        {
                            throw new ArgumentNullException(nameof(mealchoices),
                            $"Er is geen maaltijd geselecteerd voor de passagier {passenger.FirstName} {passenger.LastName}");
                        }

                        foreach (var mealchoice in mealchoices)
                        {
                            passengerFlightMeals.MealChoics.Add(
                                new FlightMealChoice
                                {
                                    FlightId = mealchoice.FlightId,
                                    MealOptionsId = mealchoice.MealOptionId
                                });
                        }

                        bookingRequest.PassengerFlightMeals.Add(passengerFlightMeals);

                    }

                    var booking = await _bookingService.CreateBookingAsync(bookingRequest);
                    return RedirectToAction("BookingConfirmation", new { id = booking.Id });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Er is iets misgegaan: {ex.Message}");
                    return View();
                }
            }


            return View();
        }

        public async Task<IActionResult> BookingConfirmation(int id)
        {
            var booking = await _bookingService.FindByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);

        }


    }
}