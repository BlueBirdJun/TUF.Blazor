using Knus.Common.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using TUF.Front.Client.Components.Common;
using TUF.Front.Client.Components.Dialogs;
using TUF.Shared.Dtos.Member;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TUF.Front.Client.Areas.Identity;

public partial class Resister
{
    private readonly CreateUserDto.Request _createUserRequest = new();

    private CustomValidation? _customValidation;
    private bool BusySubmitting { get; set; }
    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    [Parameter]
    public string Refpage { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {     
            if (DateTime.Now.ToString("yyyyMMdd") != Refpage.Split('_')[0])
            {
                Navigation.NavigateTo("/");
               return;
            }  
    }

    protected override Task OnInitializedAsync()
    {
       

        return base.OnInitializedAsync();
    }



    private async Task SubmitAsync()
    {
        if(_createUserRequest.Password != _createUserRequest.ConfirmPassword)
        {
            Snackbar.Add("비밀번호가 일치하지 않습니다.", Severity.Info);
            return;
        }

        ApiProvider<CreateUserDto> api = new ApiProvider<CreateUserDto>();
        api.BaseAddress = Config["ApiBaseUrl"];
        api.Apimeta = _createUserRequest.metadata;
        api.SendValue = JsonConvert.SerializeObject(_createUserRequest);
        var rt = await api.AsyncCallData();
        DialogOptions disableBackdropClick = new DialogOptions() { DisableBackdropClick = true };
        if (!rt.OutValue.Success)
        {    
            var parameters = new DialogParameters();
            parameters.Add("ContentText", rt.OutValue.Message);
            parameters.Add("DialogKind", DialogEnum.Info);            
            DialogService.Show<CommonDialog>("회원가입", parameters, disableBackdropClick);
        }
        else
        {
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "가입축하");
            parameters.Add("DialogKind", DialogEnum.Info);
            var drt=await DialogService.Show<CommonDialog>("회원가입", parameters, disableBackdropClick).Result;
            Navigation.NavigateTo("/login");
        }

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
