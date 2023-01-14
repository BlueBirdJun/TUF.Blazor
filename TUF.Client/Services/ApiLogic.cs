
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TUF.Domains.Dtos;
using TUF.Domains.Dtos.Member;

namespace TUF.Client.Services;
public interface IApiLogic
{
    Task<string> LoginAsync(LoginDto login);
    Task<(string Message, UserProfileDto? UserProfile)> UserProfileAsync();
    Task<string> LogoutAsync();
}

public class ApiLogic:IApiLogic
{
    private readonly IHttpClientFactory _httpClientFactory;
    public ApiLogic(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<string> LoginAsync(LoginDto login)
    {
        var client = _httpClientFactory.CreateClient("API");
        string payload = JsonSerializer.Serialize(login);
        var content = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/Auth/login", content);
        if (response.IsSuccessStatusCode)
        {
            return "Success";
        }
        else
        {
            return "failed";
        }
    }

    public async Task<(string Message, UserProfileDto? UserProfile)> UserProfileAsync()
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.GetAsync("/Auth/user-profile");
        if (response.IsSuccessStatusCode)
        {
            return ("Success", await response.Content.ReadFromJsonAsync<UserProfileDto>());
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return ("Unauthorized", null);
            }
            else
            {
                return ("Failed", null);
            }
        }
    }

    public async Task<string> LogoutAsync()
    {
        var client = _httpClientFactory.CreateClient("API");
        var response = await client.PostAsync("/Auth/logout", null);
        if (response.IsSuccessStatusCode)
        {
            return "Success";
        }
        return "Failed";
    }
}
