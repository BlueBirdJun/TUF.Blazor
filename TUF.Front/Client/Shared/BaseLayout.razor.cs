using Microsoft.AspNetCore.Components;
using MudBlazor;
using TUF.Front.Client.Infrastructure;

namespace TUF.Front.Client.Shared;

public partial class BaseLayout
{
    //private bool _themeDrawerOpen;
    private bool _rightToLeft;
    private bool _drawerOpen;
    [Parameter]
    public EventCallback<bool> OnRightToLeftToggle { get; set; }

    private MudTheme _currentTheme = new NavyTheme();
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;
    protected override async Task OnInitializedAsync()
    {
        SetCurrentTheme();
    }
    private void SetCurrentTheme()
    { 
        _rightToLeft = false;
    }

    private async Task RightToLeftToggle()
    {
        bool isRtl = true;// await ClientPreferences.ToggleLayoutDirectionAsync();
        _rightToLeft = isRtl;

        await OnRightToLeftToggle.InvokeAsync(isRtl);
    }

    private async Task DrawerToggle()
    {
        _drawerOpen = true;
    }
}
