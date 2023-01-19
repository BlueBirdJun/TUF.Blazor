using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Infrastructure.Auth;

public interface IAuthenticationService
{
    AuthProvider ProviderType { get; }

    void NavigateToExternalLogin(string returnUrl);

    Task<bool> LoginAsync(string tenantId, TokenRequest request);

    Task LogoutAsync();

    Task ReLoginAsync(string returnUrl);
}
