using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Services
{
    public interface ISeatAllocatorService
    {
        Task<List<Seat>> AllocateSeatsAsync(int flightId, int passengerCount, bool isBusiness);
    }
 }
