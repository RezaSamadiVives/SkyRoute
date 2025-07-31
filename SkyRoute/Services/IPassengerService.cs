using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public interface IPassengerService
    {
        void AddPassenger(PassengerListVM model);
        void RemovePassengerAt(PassengerListVM model, int index);
        void UpdatePassengerIdsAndUser(PassengerListVM model, string userId);
        
    }
}