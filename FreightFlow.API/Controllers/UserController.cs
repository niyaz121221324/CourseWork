using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class UserController : BaseCrudController<User>
{
    public UserController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}