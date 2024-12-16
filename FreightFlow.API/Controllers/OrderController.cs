using FreightFlow.DAL.Contexts;
using FreightFlow.DAL.Models;

namespace FreightFlow.API.Controllers;

public class OrderController : BaseCrudController<Order>
{
    public OrderController(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}