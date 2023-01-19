using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TUF.Application.Common.Events;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.Domains.Common.Exceptions;
using TUF.Infrastructure.Auth;
using TUF.Shared.Authorization;
//using TUF.Shared.Authorization;
//using TUF.Shared.Identitys;
//using TUF.Shared.Infrastructure.Auth;

namespace TUF.Application.Identity.Roles;
public interface IRoleService : ITransient
{
    Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<bool> ExistsAsync(string roleName, string? excludeId);

    Task<RoleDto> GetByIdAsync(string id);

    Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken);

    Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request);

    Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken);

    Task<string> DeleteAsync(string id);
}
//internal class RoleService:IRoleService
//{
//    private readonly RoleManager<ApplicationRole> _roleManager;
//    private readonly UserManager<ApplicationUser> _userManager;
//    private readonly IdentityContext _db;
//    private readonly ICurrentUser _currentUser;    
//    private readonly IEventPublisher _events;

//    public RoleService(
//        RoleManager<ApplicationRole> roleManager,
//        UserManager<ApplicationUser> userManager,
//        IdentityContext db,        
//        ICurrentUser currentUser,
        
//        IEventPublisher events)
//    {
//        _roleManager = roleManager;
//        _userManager = userManager;
//        _db = db;        
//        _currentUser = currentUser;        
//        _events = events;
//    }
//    public async Task<List<RoleDto>> GetListAsync(CancellationToken cancellationToken) =>
//        (await _roleManager.Roles.ToListAsync(cancellationToken))
//            .Adapt<List<RoleDto>>();

//    public async Task<int> GetCountAsync(CancellationToken cancellationToken) =>
//        await _roleManager.Roles.CountAsync(cancellationToken);

//    public async Task<bool> ExistsAsync(string roleName, string? excludeId) =>
//        await _roleManager.FindByNameAsync(roleName)
//            is ApplicationRole existingRole
//            && existingRole.Id != excludeId;

//    public async Task<RoleDto> GetByIdAsync(string id) =>
//        await _db.Roles.SingleOrDefaultAsync(x => x.Id == id) is { } role
//            ? role.Adapt<RoleDto>()
//            : throw new NotFoundException("Role Not Found");

//    public async Task<RoleDto> GetByIdWithPermissionsAsync(string roleId, CancellationToken cancellationToken)
//    {
//        var role = await GetByIdAsync(roleId);

//        role.Permissions = await _db.RoleClaims
//            .Where(c => c.RoleId == roleId && c.ClaimType == TUFClaims.Permission)
//            .Select(c => c.ClaimValue)
//            .ToListAsync(cancellationToken);

//        return role;
//    }

//    public async Task<string> CreateOrUpdateAsync(CreateOrUpdateRoleRequest request)
//    {
//        if (string.IsNullOrEmpty(request.Id))
//        {
//            // Create a new role.
//            var role = new ApplicationRole(request.Name, request.Description);
//            var result = await _roleManager.CreateAsync(role);

//            if (!result.Succeeded)
//            {
//                throw new InternalServerException("Register role failed", result.GetErrors());
//            }

//            //await _events.PublishAsync(new ApplicationRoleCreatedEvent(role.Id, role.Name));

//            return string.Format("Role {0} Created.", request.Name);
//        }
//        else
//        {
//            // Update an existing role.
//            var role = await _roleManager.FindByIdAsync(request.Id);

//            _ = role ?? throw new NotFoundException("Role Not Found");

//            if (TUFRoles.IsDefault(role.Name))
//            {
//                throw new ConflictException(string.Format("Not allowed to modify {0} Role.", role.Name));
//            }

//            role.Name = request.Name;
//            role.NormalizedName = request.Name.ToUpperInvariant();
//            role.Description = request.Description;
//            var result = await _roleManager.UpdateAsync(role);

//            if (!result.Succeeded)
//            {
//                throw new InternalServerException("Update role failed", result.GetErrors());
//            }

//            //await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name));

//            return string.Format("Role {0} Updated.", role.Name);
//        }
//    }

//    public async Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request, CancellationToken cancellationToken)
//    {
//        var role = await _roleManager.FindByIdAsync(request.RoleId);
//        _ = role ?? throw new NotFoundException("Role Not Found");
//        if (role.Name == TUFRoles.Admin)
//        {
//            throw new ConflictException("Not allowed to modify Permissions for this Role.");
//        }
         

//        var currentClaims = await _roleManager.GetClaimsAsync(role);

//        // Remove permissions that were previously selected
//        foreach (var claim in currentClaims.Where(c => !request.Permissions.Any(p => p == c.Value)))
//        {
//            var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
//            if (!removeResult.Succeeded)
//            {
//                throw new InternalServerException("Update permissions failed.", removeResult.GetErrors());
//            }
//        }

//        // Add all permissions that were not previously selected
//        foreach (string permission in request.Permissions.Where(c => !currentClaims.Any(p => p.Value == c)))
//        {
//            if (!string.IsNullOrEmpty(permission))
//            {
//                _db.RoleClaims.Add(new ApplicationRoleClaim
//                {
//                    RoleId = role.Id,
//                    ClaimType = TUFClaims.Permission,
//                    ClaimValue = permission,
//                    CreatedBy = _currentUser.GetUserId().ToString()
//                });
//                await _db.SaveChangesAsync(cancellationToken);
//            }
//        }

//        //await _events.PublishAsync(new ApplicationRoleUpdatedEvent(role.Id, role.Name, true));

//        return "Permissions Updated.";
//    }

//    public async Task<string> DeleteAsync(string id)
//    {
//        var role = await _roleManager.FindByIdAsync(id);

//        _ = role ?? throw new NotFoundException("Role Not Found");

//        if (TUFRoles.IsDefault(role.Name))
//        {
//            throw new ConflictException(string.Format("Not allowed to delete {0} Role.", role.Name));
//        }

//        if ((await _userManager.GetUsersInRoleAsync(role.Name)).Count > 0)
//        {
//            throw new ConflictException(string.Format("Not allowed to delete {0} Role as it is being used.", role.Name));
//        }

//        await _roleManager.DeleteAsync(role);

//        //await _events.PublishAsync(new ApplicationRoleDeletedEvent(role.Id, role.Name));

//        return string.Format("Role {0} Deleted.", role.Name);
//    }
//}
