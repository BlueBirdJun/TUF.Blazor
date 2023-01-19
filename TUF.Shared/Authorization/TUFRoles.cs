using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Authorization;

public static class TUFRoles
{
    public const string Admin = nameof(Admin);
    public const string Superuser = nameof(Superuser);
    public const string VipUser = nameof(VipUser);
    public const string NormarUser = nameof(NormarUser);
    public const string RookieUser = nameof(RookieUser);
    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
    Admin,
    Superuser,
    NormarUser, RookieUser
});

    public static bool IsDefault(string roleName) => DefaultRoles.Any(r => r == roleName);
}
