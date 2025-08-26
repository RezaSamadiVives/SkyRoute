using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.Controllers
{
    [Authorize]
    public class BookingsController(IBookingService _bookingService,
         IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var bookings = await _bookingService.GetAllBookingAsyncByUser(userId);

            var model = _mapper.Map<List<BookingVM>>(bookings);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelBooking(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "Ongeldige boeking.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _bookingService.CancelBookingAsync(id);
                TempData["Success"] = "Je boeking is succesvol geannuleerd.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Annuleren mislukt: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


    }
}