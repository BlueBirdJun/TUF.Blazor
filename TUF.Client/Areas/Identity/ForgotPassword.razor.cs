using TUF.Client.Components.Common;
using TUF.Shared.Dtos.Member;

namespace TUF.Client.Areas.Identity;

public partial class ForgotPassword
{
    private readonly ForgotPasswordDto _forgotPasswordRequest = new();
    private CustomValidation? _customValidation;
    private bool BusySubmitting { get; set; }
    private async Task SubmitAsync()
    {

    }

}

