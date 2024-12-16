using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class TripController : BaseCrudController<Trip>
{
    public TripController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}