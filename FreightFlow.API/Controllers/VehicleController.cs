using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class VehicleController : BaseCrudController<Vehicle>
{
    public VehicleController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}