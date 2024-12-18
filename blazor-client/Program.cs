using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using blazor_client;
using blazor_client.Repositories;
using blazor_client.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Correctly register the generic repository
builder.Services.AddScoped<IRepository<User>, Repository<User>>();

// Register HttpClient as a singleton with the correct base address
builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri("http://freightflow.api:8080") });

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();