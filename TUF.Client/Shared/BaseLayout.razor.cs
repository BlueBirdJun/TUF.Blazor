using MudBlazor;

namespace TUF.Client.Shared;

public partial class BaseLayout
{
    private bool _themeDrawerOpen;
    private bool _rightToLeft;
    protected override async Task OnInitializedAsync()
    {     

        //Snackbar.Add("Like this boilerplate? ", Severity.Normal, config =>
        //{
        //    config.BackgroundBlurred = true;
        //    config.Icon = Icons.Custom.Brands.GitHub;
        //    config.Action = "Star us on Github!";
        //    config.ActionColor = Color.Primary;
        //    config.Onclick = snackbar =>
        //    {
        //        //Navigation.NavigateTo("https://github.com/fullstackhero/blazor-wasm-boilerplate");
        //        return Task.CompletedTask;
        //    };
        //});
    }

}
