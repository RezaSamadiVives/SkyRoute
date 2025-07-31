using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class PassengerValidator : IPassengerValidator
    {
        public void Validate(PassengerListVM model, ModelStateDictionary modelState)
        {

            if (model == null || model.Passengers == null || model.Passengers.Count == 0)
            {
                modelState.AddModelError("", "Je moet minstens één passagier toevoegen.");
                return;
            }

            int hoofdpassagiers = model.Passengers.Count(p => !p.IsFellowPassenger);
            if (hoofdpassagiers == 0)
            {
                modelState.AddModelError("", "Geef aan wie de hoofdpassagier is.");
            }
            else if (hoofdpassagiers > 1)
            {
                modelState.AddModelError("", "Er mag maar één hoofdpassagier zijn.");
            }
        }
    }
}