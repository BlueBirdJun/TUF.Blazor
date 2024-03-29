﻿using Daniel.Common.Interfaces;
using Hangfire.States;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TUF.Database.Identity.Models;
using TUF.Domains.Common.Exceptions;

using TUF.Infrastructure.Auth;
using TUF.Infrastructure.Auth.Jwt;
using TUF.Shared.Authorization;
using TUF.Shared.Dtos;

namespace TUF.Infrastructure.Identity.Tokens;

public interface ITokenService : ITransient
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> LoginTokenAsync(LoginDto.Request request, string ipAddress);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}

internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;

    public TokenService(
       UserManager<ApplicationUser> userManager,
       IOptions<JwtSettings> jwtSettings, 
       IOptions<SecuritySettings> securitySettings)
    {
        _userManager = userManager;        
        _jwtSettings = jwtSettings.Value;        
        _securitySettings = securitySettings.Value;
    }
    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken)
    {
        if ( await _userManager.FindByEmailAsync(request.Email.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException("Authentication Failed.");
        }
        if (!user.IsActive.Value)
        {
            throw new UnauthorizedException("User Not Active. Please contact the administrator.");
        }
        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException("E-Mail not confirmed.");
        }  
        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }
    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        string? userEmail = userPrincipal.GetEmail();
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user is null)
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid Refresh Token.");
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }
    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

   

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
      new List<Claim>
      {
            
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(TUFClaims.FullName, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.LastName ?? string.Empty),            
            //new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(TUFClaims.IpAddress, ipAddress),
            //new(TUFClaims.ProfilePicture, user.strProfilePicture),
            new(TUFClaims.NickName, user.NickName ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new(ClaimTypes.Role, TUFRoles.Admin ?? string.Empty),
            new(TUFClaims.MemberType, user.MemberType ?? string.Empty)
      };
    private string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException("Invalid Token.");
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    public async Task<TokenResponse> LoginTokenAsync(LoginDto.Request request, string ipAddress)
    {
        if (await _userManager.FindByNameAsync(request.UserId.Trim().Normalize()) is not { } user
            || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException("Authentication Failed.");
        }
        if (!user.IsActive.Value)
        {
            throw new UnauthorizedException("User Not Active. Please contact the administrator.");
        }
        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException("E-Mail not confirmed.");
        }
        return await GenerateTokensAndUpdateUser(user, ipAddress);

        
    }
}