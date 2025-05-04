using Microsoft.EntityFrameworkCore;
using SkyRoute.Domains.Data;
using SkyRoute.Domains.Entities;
using SkyRoute.Repositories.Interfaces;

namespace SkyRoute.Repositories.Repositories
{
    public class CityDAO(SkyRouteDbContext context) : BaseDAO<City>(context)
    {

    }
}
