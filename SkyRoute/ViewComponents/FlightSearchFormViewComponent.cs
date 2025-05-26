using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkyRoute.Domains.Entities;
using SkyRoute.Services.Interfaces;
using SkyRoute.ViewModels;

namespace SkyRoute.ViewComponents
{
    public class FlightSearchFormViewComponent : ViewComponent
    {
        private readonly IService<City> _cityService;

        public FlightSearchFormViewComponent(IService<City> cityService)
        {
            _cityService = cityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string formController = "Home", string formAction = "Index", object? model = null)
        {
            var vm = model as FlightSearchFormVM ?? new FlightSearchFormVM();
            await PopulateCities(vm);
            if(vm.ReturnDate.HasValue)
            {
                vm.ReturnDate = vm.ReturnDate;
            }

            ViewData["FormController"] = formController;
            ViewData["FormAction"] = formAction;

            return View("FlightSearchForm", vm);
        }

        private async Task PopulateCities(FlightSearchFormVM model)
        {
            var cities = await _cityService.GetAllAsync();
            model.Cities =
            [
                .. from city in cities
                   select new SelectListItem { Value = city.Id.ToString(), Text = city.Name },
            ];
        }
    }
}
