using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TUF.Application;
using TUF.HostApi.Configurations;
using TUF.HostApi.Controllers;
using TUF.Infrastructure.OpenApi;

[assembly: ApiConventionType(typeof(TUFApiConventions))]
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Host.AddConfigurations();
builder.Host.UseSerilog((_, config) =>
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddControllersWithViews();//.AddFluentValidation();
 

builder.Services.AddMediator(); 
//builder.Services.AddOpenApiDocumentation(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPersistenceContexts(builder.Configuration);
builder.Services.AddApplicationInfrastructure(builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseInfrastructure(builder.Configuration);
 
app.MapEndpoints();
 
app.Run();
 