using Autofac.Core;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Serilog;
using TUF.Api.Configurations;
using TUF.Api.Controllers;
using TUF.Application;
using TUF.Database.DbContexts;

[assembly: ApiConventionType(typeof(TUFApiConventions))]

//string CorsName = "TufCors";
string CorsPolicy = nameof(CorsPolicy);

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddConfigurations();
builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllers();//.AddFluentValidation();
builder.Services.AddApplication(); //mediat 


//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
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
app.UseInfrastructure(builder.Configuration); 
app.MapEndpoints(); 
app.Run();
 