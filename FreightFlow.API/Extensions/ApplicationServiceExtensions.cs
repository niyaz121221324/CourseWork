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

        ConfigurerCorsPolicy(services);

        return services;
    }

    private static void ConfigurerCorsPolicy(IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000", "http://blazor-client:80", "http://localhost:80");
            });
        });
    }
}