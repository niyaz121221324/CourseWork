using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class DriverController : BaseCrudController<Driver>
{
    public DriverController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}