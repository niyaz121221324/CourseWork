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
        var allowedOrigins = new[]
        {
            "http://localhost:3000",
            "http://blazor-client:80",
            "http://localhost:80",
            "https://glider-dear-hog.ngrok-free.app"
        };
        
        services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(allowedOrigins);
            });
        });
    }
}