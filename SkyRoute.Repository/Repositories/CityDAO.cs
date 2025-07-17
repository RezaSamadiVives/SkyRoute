using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;

namespace SkyRoute.Repositories.Repositories
{
    public class CityDAO(SkyRouteDbContext context) : BaseDAO<City>(context)
    {

    }
}
