using SkyRoute.ViewModels;

namespace SkyRoute.Services
{
    public class PassengerService : IPassengerService
    {
        public void AddPassenger(PassengerListVM model)
        {
            model.Passengers.Add(new PassengerVM());
        }

        public void RemovePassengerAt(PassengerListVM model, int index)
        {
            if (index >= 0 && index < model.Passengers.Count)
            {
                model.Passengers.RemoveAt(index);
            }
        }

        public void UpdatePassengerIdsAndUser(PassengerListVM model, string userId)
        {
            for (int i = 0; i < model.Passengers.Count; i++)
            {
                model.Passengers[i].UserId = userId;
                model.Passengers[i].Id = i + 1;
            }
        }

    }
}