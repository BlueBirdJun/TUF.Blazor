using Knus.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TUF.Domains.Common.Exceptions;
using TUF.Shared.Dtos;

namespace TUF.Infrastructure.Auth.Jwt;

public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings;

    public ConfigureJwtBearerOptions(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(string.Empty, options);
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
        {
            return;
        }

        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                if (!context.Response.HasStarted)
                { 
                    context.Response.StatusCode = 401;//HttpStatusCode.Unauthorized.ToString().ToInt();
                    var r = new ApiError() { ApiEnum= ApiEnum.UnAuthorized,Message="JWT KEY NO" };
                    context.Response.ContentType = "application/json";                    
                    context.Response.WriteAsync(JsonConvert.SerializeObject(r));                    
                    return Task.CompletedTask;                     
                }                
                return Task.CompletedTask;
            },
            OnForbidden = context => {
                context.Response.StatusCode = 401;//HttpStatusCode.Unauthorized.ToString().ToInt();
                var r = new ApiError() { ApiEnum = ApiEnum.OnForbidden, Message = "권한없음" };
                context.Response.ContentType = "application/json";
                context.Response.WriteAsync(JsonConvert.SerializeObject(r));
                return Task.CompletedTask;
            }
            //_ => throw new ForbiddenException("You are not authorized to access this resource.")
            ,
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    }
}