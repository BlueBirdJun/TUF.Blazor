using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Infrastructure.Auth;

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }

    public ApplicationRole(string name, string? description = null)
        : base(name)
    {
        Description = description;
        NormalizedName = name.ToUpperInvariant();
    }
}