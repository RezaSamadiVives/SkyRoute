using SkyRoute.Domains.Enums;

namespace SkyRoute.ViewModels
{

    public class BookingVM
    {
        public int Id { get; set; }
        public string Reference { get; set; } = "";
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public string? Note { get; set; }
        
        public ICollection<TicketVM> Tickets { get; set; } = [];
    }
 }
