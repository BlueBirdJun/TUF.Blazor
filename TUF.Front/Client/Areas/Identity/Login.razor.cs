using Blazored.LocalStorage;
using Knus.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TUF.Front.Client.Common;
using TUF.Front.Client.Components.Common;
using TUF.Front.Client.Components.Dialogs;
using TUF.Front.Client.Components.Member;
using TUF.Front.Client.Services;
using TUF.Front.Client.Shared;
using TUF.Shared.Dtos;
using TUF.Shared.Dtos.Member;
using static MudBlazor.Colors;
using static System.Net.WebRequestMethods;

namespace TUF.Front.Client.Areas.Identity;

public partial class Login
{
    [Inject]
    public IApiLogic _apiLogic { get; set; } = default;
    
    [Inject]
    public NavigationManager _navigationManager { get; set; } = default;
    //[Inject]
    //public ILocalStorageService _localStorageService { get; set; } = default;
    private CustomValidation? _customValidation;
    public bool BusySubmitting { get; set; } = false;
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    protected override async Task OnInitializedAsync()
    {
        //await GetTest();   
    }

    private async Task GetTest()
    {  

        CreateUserDto.Request data = new CreateUserDto.Request();
        data.UserName = "안녕";
        data.UserID = "aa";
        data.UserEmail = "bb@da.net";
        data.Password = "cc";
        data.ConfirmPassword = "cc";
        //var c = await Http.PostAsJsonAsync<CreateUserDto.Request>("/api/users/resister", data);
        //var rt=await c.Content.ReadFromJsonAsync<DtoBase<string, string>>();

        ApiProvider<CreateUserDto> api = new ApiProvider<CreateUserDto>();
        api.BaseAddress = Config["ApiBaseUrl"];
        api.Apimeta = data.metadata;
        //api.HttpMethodValue = data.httpmethod;
        //api.QueryPath = data.UrlPath;
        
        api.SendValue = JsonConvert.SerializeObject(data);
        var rt1 = await api.AsyncCallData();


        

        DialogOptions disableBackdropClick = new DialogOptions() { DisableBackdropClick = true };
        var parameters = new DialogParameters();
        parameters.Add("ContentText", "Are you sure you want to remove thisguy@emailz.com from this account?");
        parameters.Add("ButtonText", "Yes");
        parameters.Add("Color", Color.Success);

        var r = DialogService.Show<CommonDialog>("ddd",parameters,disableBackdropClick);

        //Snackbar.Add(rt.Message);
        
        //ApiHelper.ExecuteCallGuardedAsync<DtoBase<string,string>>()
    }

    private LoginDto.Request loginModel = new();

    private async Task UserLogin()
    {
        //var message = await _apiLogic.LoginAsync(loginModel);
        //if (message == "Success")
        //{
        //    await _localStorageService.SetItemAsStringAsync("isauthenticated", "true");
        //    _navigationManager.NavigateTo("/", true);
        //}
    }
    private async Task SubmitAsync()
    {
        //var message = await _apiLogic.LoginAsync(loginModel);
        //if (message == "Success")
        //{
        //    await _localStorageService.SetItemAsStringAsync("isauthenticated", "true");
        //    _navigationManager.NavigateTo("/", true);
        //}
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

    private async Task ShowCreateUser()
    {
        DialogService.ShowModal<CreateUser>(null);

        //var dialog = DialogService.ShowModa)
        Snackbar.Add("ff", Severity.Warning);
    }
}
