using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TUF.Client;
using TUF.Client.Areas.Identity;
using TUF.Client.Handler;
using TUF.Client.Providers;
using TUF.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services
.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddHttpClient("API", options => {
    options.BaseAddress = new Uri("https://localhost:7008/");
    
})
.AddHttpMessageHandler<CookieHandler>();

builder.Services.AddScoped<IApiLogic, ApiLogic>();

var host = builder.Build(); 

await host.RunAsync();
