using FreightFlow.DAL.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FreightFlow.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}