using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using blazor_client;
using blazor_client.Repositories;
using blazor_client.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configure HttpClient for dependency injection with the base address
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8080/api") });

// Register the repository as a scoped service for dependency injection
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

// Add root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();