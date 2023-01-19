using Daniel.Common.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Application.Identity.Roles;

public class UpdateRolePermissionsRequest
{
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;
}

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty();
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}