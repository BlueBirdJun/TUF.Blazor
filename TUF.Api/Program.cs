using Autofac.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using TUF.Api.Configurations;
using TUF.Application;
using TUF.Database.DbContexts;

//string CorsName = "TufCors";
string CorsPolicy = nameof(CorsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddConfigurations();
builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllersWithViews();
builder.Services.AddApplication(); //mediat 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication(
//    CookieAuthenticationDefaults.AuthenticationScheme
//)
//.AddCookie();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistenceContexts(builder.Configuration);
builder.Services.AddApplicationInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy,
    builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(options => true)
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseInfrastructure(builder.Configuration);
//app.UseCors(CorsPolicy); //CORS
//app.UseHttpsRedirection(); //HTTPS
//app.UseAuthentication(); //
//app.UseAuthorization();
//app.MapControllers().RequireAuthorization();
//builder.MapHealthCheck();
//builder.MapNotifications();
app.MapEndpoints();
//app.MapFallbackToFile("index.html");

 

app.Run();


//app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();
//app.UseStaticFiles();
//app.UseRouting();
//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");
//app.Run();

/*
var builder = WebApplication.CreateBuilder(args);
string CorsName = "TufCors";
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme
)
.AddCookie();

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsName,
    builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(options => true)
            .AllowCredentials();
    });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseCors(CorsName);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseBlazorFrameworkFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
*/