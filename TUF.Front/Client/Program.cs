//using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using TUF.Front.Client;

using TUF.Front.Client.Services;
using Blazored.SessionStorage;
using Blazored.LocalStorage;
using TUF.Front.Client.Common;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClientServices(builder.Configuration);
 
await builder.Build().RunAsync();


//builder.Services.AddOptions();
//builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationService>()
//                .AddScoped(sp => (IAuthenticationService)sp.GetRequiredService<AuthenticationStateProvider>())
//                .AddScoped(sp => (IAccessTokenProvider)sp.GetRequiredService<AuthenticationStateProvider>());
//.AddScoped<IAccessTokenProviderAccessor, AccessTokenProviderAccessor>();
//.AddScoped<JwtAuthenticationHeaderHandler>()


//builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationService>();
//builder.Services.AddAuthorizationCore();
//builder.Services
//.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

//builder.Services.AddScoped<CookieHandler>();

//builder.Services.AddMudServices(configuration =>
//{
//    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
//    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
//    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
//    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
//    configuration.SnackbarConfiguration.ShowCloseIcon = false;
//});


//builder.Services.AddHttpClient("API", options =>
//{
//    options.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
//}); 
//Daniel.Common.BootStrap.BaseAddress = builder.Configuration["ApiBaseUrl"];
//.AddHttpMessageHandler<CookieHandler>();

//builder.Services.AddScoped<IApiLogic, ApiLogic>();
//builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();



