using Microsoft.AspNetCore.Mvc.ModelBinding;
using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IPassengerValidator
    {
           void Validate(PassengerListVM model, ModelStateDictionary modelState); 
    }
}