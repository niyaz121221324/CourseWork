using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class ClientController : BaseCrudController<Client>
{
    public ClientController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}