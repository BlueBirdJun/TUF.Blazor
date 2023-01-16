using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using TUF.Client.Components.Common;
using TUF.Client.Services;
using TUF.Shared.Dtos;

namespace TUF.Client.Areas.Identity;

public partial class Login
{
    [Inject]
    public IApiLogic _apiLogic { get; set; } = default;
    [Inject]
    public AuthenticationStateProvider _authStateProvider { get; set; } = default;
    [Inject]
    public NavigationManager _navigationManager { get; set; } = default;
    [Inject]
    public ILocalStorageService _localStorageService { get; set; } = default;
    private CustomValidation? _customValidation;
    public bool BusySubmitting { get; set; } = false;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {

    }
    private LoginDto loginModel = new();

    private async Task UserLogin()
    {
        var message = await _apiLogic.LoginAsync(loginModel);
        if (message == "Success")
        {
            await _localStorageService.SetItemAsStringAsync("isauthenticated", "true");
            _navigationManager.NavigateTo("/", true);
        }
    }
    private async Task SubmitAsync()
    {

    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }

    }
}
