using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Repositories
{
    public class AirlineDAO(SkyRouteDbContext context) : BaseDAO<Airline>(context)
    {
    }
}
