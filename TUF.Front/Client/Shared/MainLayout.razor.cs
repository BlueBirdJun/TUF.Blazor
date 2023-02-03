using Microsoft.AspNetCore.Components;
using MudBlazor;
using TUF.Front.Client.Components.Dialogs;
using TUF.Front.Client.Services;

namespace TUF.Front.Client.Shared;

public partial class MainLayout
{
    [Inject]
    JwtAuthenticationService jwtprovider { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
    
    [Parameter]
    public EventCallback<bool> OnRightToLeftToggle { get; set; }  
    private bool _drawerOpen=true;
    private bool _rightToLeft;

    protected override async Task OnInitializedAsync()
    {

    }
    private async Task RightToLeftToggle()
    {
        bool isRtl = true; //await ClientPreferences.ToggleLayoutDirectionAsync();
        _rightToLeft = isRtl;
        await OnRightToLeftToggle.InvokeAsync(isRtl);
    } 
    private async Task DrawerToggle()
    {
        _drawerOpen = _drawerOpen?false: true; // await ClientPreferences.ToggleDrawerAsync();
    }
    private async Task Logout()
    {
        DialogOptions disableBackdropClick = new DialogOptions() { DisableBackdropClick = true };

        var parameters = new DialogParameters();
        parameters.Add("ContentText", "로그아웃 하게?");
        parameters.Add("DialogKind", DialogEnum.YesOrNo);
        var drt =await   DialogService.Show<CommonDialog>("회원가입", parameters, disableBackdropClick).Result;
        if (!drt.Canceled)
        {
            await jwtprovider.Logout();            
        }
        //
    }
}
