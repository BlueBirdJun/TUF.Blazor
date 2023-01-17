using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Authorization;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    
    public MustHavePermissionAttribute(string action, string resource) =>
        Policy = TUFPermission.NameFor(action, resource);
}

public record TUFPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
   

    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
}

//public class MustHavePermissionAttribute : AuthorizeAttribute
//{


//    public MustHavePermissionAttribute(string action, string resource) =>
//        Policy = TUFPermission.NameFor(action, resource);
//}