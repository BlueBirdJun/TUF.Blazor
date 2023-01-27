using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using MudBlazor;
using TUF.Client.Components.Common;
using TUF.Shared.Dtos.Member;

namespace TUF.Client.Areas.Identity;

public partial class Resister
{
    private readonly CreateUserDto _createUserRequest = new();

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
        string cc = $"{_createUserRequest.UserID} {_createUserRequest.UserName} ";

        
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
