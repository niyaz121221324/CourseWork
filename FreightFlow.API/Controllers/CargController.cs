using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class CargoController : BaseCrudController<Cargo>
{
    public CargoController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}