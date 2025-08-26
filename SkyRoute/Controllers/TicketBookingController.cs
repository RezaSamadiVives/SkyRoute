using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Domains.Entities;
using SkyRoute.Domains.Models;
using SkyRoute.Helpers;
using SkyRoute.Services;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class TicketBookingController(
        IShoppingcartService _shoppingcartService,
        IBookingService _bookingService,
        ITicketService _ticketService,
        IEmailSender _emailSender,
         IMapper _mapper) : Controller
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
                    else
                    {
                        bookingRequest.SegmentIdRetour = null;
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
                    await SendEmailAsync(booking);

                    return RedirectToAction("BookingConfirmation", new { id = booking.Id });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Er is iets misgegaan: {ex.Message}");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Ongeldige sessie. Probeer opnieuw te boeken.");
                return View();
            }
        }

        private async Task SendEmailAsync(Booking booking)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return; // geen emailadres beschikbaar
            }

            // Haal alle tickets van deze booking inclusief navigation properties
            var tickets = await _ticketService.GetAllTicketsByBooking(booking.Id);
            if (tickets == null || tickets.Count == 0)
            {
                return; // geen tickets
            }

            foreach (var ticket in tickets)
            {
                // Volledige naam passagier
                var passengerFullName = string.Join(" ",
                    ticket.Passenger?.FirstName ?? "",
                    string.IsNullOrEmpty(ticket.Passenger?.MiddelName) ? "" : ticket.Passenger.MiddelName,
                    ticket.Passenger?.LastName ?? ""
                ).Trim();

                var travelClass = ticket.IsBusiness ? "Business" : "Economy";
                var mealOption = ticket.MealOption?.Name ?? "Geen";

                // Veilige datumformattering met InvariantCulture
                var departureTime = ticket.Flight?.FlightDate.ToString("t") + ticket.Flight?.DepartureTime.ToString("G")?? "Onbekend";
                var arrivalTime = ticket.Flight?.ArrivalDate.ToString("t") + ticket.Flight?.ArrivalTime.ToString("G") ?? "Onbekend";
                var fromCity = ticket.Flight?.FromCity?.Name ?? "Onbekend";
                var toCity = ticket.Flight?.ToCity?.Name ?? "Onbekend";
                var bookingDate = booking.BookingDate.ToString("G");

                // Prijs correct als string
                var price = ticket.Price.ToString("F2", CultureInfo.InvariantCulture);

                // E-mail bericht
                var message = $@"
            <div style='font-family: Arial, sans-serif;'>
                <h2 style='color:#0079CF;'>Jouw Vluchtticket</h2>
                <p>Bedankt voor je boeking! Hieronder vind je de gegevens van je ticket.</p>

                <div style='border:1px solid #0079CF; padding:15px; border-radius:10px; background-color:#f1f8ff; margin-bottom:20px;'>
                    <h4>Boeking</h4>
                    <p><strong>Referentie:</strong> {booking.Reference}</p>
                    <p><strong>Boekingsdatum:</strong> {bookingDate}</p>

                    <h4>Passagier</h4>
                    <p>{passengerFullName}</p>
                    <p><strong>Klasse:</strong> {travelClass}</p>

                    <h4>Vluchtgegevens</h4>
                    <p>
                        <strong>Vluchtnummer:</strong> {ticket.Flight?.FlightNumber ?? "Onbekend"} <br />
                        <strong>Vertrek:</strong> {fromCity} – {departureTime} <br />
                        <strong>Aankomst:</strong> {toCity} – {arrivalTime}
                    </p>

                    <h4>Stoel & Maaltijd</h4>
                    <p><strong>Stoel:</strong> {ticket.Seat?.SeatNumber ?? "Onbekend"} ({travelClass})</p>
                    <p><strong>Maaltijd:</strong> {mealOption}</p>

                    <p><strong>Prijs:</strong> € {price}</p>
                </div>
            </div>";

                // Verstuur e-mail
                await _emailSender.SendEmailAsync(userEmail, $"Jouw ticket {ticket.Flight?.FlightNumber ?? "Onbekend"} – {passengerFullName}", message);
            }
        }

        public async Task<IActionResult> BookingConfirmation(int id)
        {
            var booking = await _bookingService.FindByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<BookingVM>(booking);
            return View(model);

        }


    }
}