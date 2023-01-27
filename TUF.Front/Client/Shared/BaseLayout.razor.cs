using MudBlazor;
using TUF.Front.Client.Infrastructure;

namespace TUF.Front.Client.Shared;

public partial class BaseLayout
{
    private bool _themeDrawerOpen;
    private bool _rightToLeft;
    private MudTheme _currentTheme = new DarkTheme();
    protected override async Task OnInitializedAsync()
    {
        SetCurrentTheme();
    }
    private void SetCurrentTheme()
    {

        _rightToLeft = false;
    }
}
