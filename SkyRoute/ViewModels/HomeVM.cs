using Microsoft.AspNetCore.Mvc.Rendering;

namespace SkyRoute.ViewModels
{
    public class HomeVM
    {
        public required List<SelectListItem> Cities { get; set; }
    }
}
