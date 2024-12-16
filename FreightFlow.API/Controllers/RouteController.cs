using FreightFlow.DAL.Contexts;

namespace FreightFlow.API.Controllers;

public class RouteController : BaseCrudController<DAL.Models.Route>
{
    public RouteController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}