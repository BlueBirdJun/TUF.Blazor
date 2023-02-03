using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using TUF.Front.Client.Common;
using TUF.Shared.Dtos;

namespace TUF.Front.Client.Services;

public class JwtAuthenticationService : AuthenticationStateProvider
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigation;
    private readonly ITokenService _tokenService;
    public JwtAuthenticationService(ILocalStorageService localStorage, NavigationManager navigation, ITokenService tokenService)
    {
        _localStorage = localStorage;
        _navigation = navigation;
        _tokenService = tokenService;
    }

    public override async  Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string cachedToken = await GetCachedAuthTokenAsync();
        if (string.IsNullOrWhiteSpace(cachedToken))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));        
        var claimsIdentity = new ClaimsIdentity(GetClaimsFromJwt(cachedToken), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(claimsIdentity));             
    }

    public async Task<(bool, string)> LoginAsync(LoginDto.Request dto)
    {
        var rt = await _tokenService.GetTokenAsync(dto);
        if (!rt.Success)
            return (false,rt.Message); 
        else
        {
            //로그인 처리 
            await CacheAuthTokens(rt.OutPutValue.Token, rt.OutPutValue.RefreshToken); 
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return (true, "");
        } 
    }

    public async Task Logout()
    {
        await ClearCacheAsync();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        _navigation.NavigateTo("/login");
    }
    public async Task ReLoginAsync(string returnUrl)
    {
        await Logout();
        _navigation.NavigateTo(returnUrl);
    }


    private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs is not null)
        {
            keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);
            if (roles is not null)
            {
                string? rolesString = roles.ToString();
                if (!string.IsNullOrEmpty(rolesString))
                {
                    if (rolesString.Trim().StartsWith("["))
                    {
                        string[]? parsedRoles = JsonSerializer.Deserialize<string[]>(rolesString);

                        if (parsedRoles is not null)
                        {
                            claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rolesString));
                    }
                }
                keyValuePairs.Remove(ClaimTypes.Role);
            } 
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));
        } 
        return claims;
    }


    private async ValueTask CacheAuthTokens(string? token, string? refreshToken)
    {
        await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
        await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
    }
    private async Task ClearCacheAsync()
    {
        await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.Permissions);
    }

    private byte[] ParseBase64WithoutPadding(string payload)
    {
        payload = payload.Trim().Replace('-', '+').Replace('_', '/');
        string base64 = payload.PadRight(payload.Length + ((4 - (payload.Length % 4)) % 4), '=');
        return Convert.FromBase64String(base64);
    }
    private ValueTask<string> GetCachedAuthTokenAsync() =>
        _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
    private async Task UpdateAhthor()
    {
        ClaimsPrincipal claimsPrincipal;
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "안녕"),
                    new Claim(ClaimTypes.Role, "Admin")
                }));

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
