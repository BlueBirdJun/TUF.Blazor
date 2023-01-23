using Azure.Core;
using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TUF.Application.Common.Caching;
using TUF.Application.Identity.Users;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.Domains.Common.Exceptions;
using TUF.Domains.Entities;
using TUF.Infrastructure.Auth;
using TUF.Infrastructure.Common;

using TUF.Shared.Authorization;
using TUF.Shared.Events;

namespace TUF.Infrastructure.Identity.Users;

public interface IUserService : ITransient
{
    //Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken);

    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    //Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    //Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);

    //Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    //Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

    //Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    //Task<string> CreateAsync(CreateUserRequest request, string origin);
    //Task UpdateAsync(UpdateUserRequest request, string userId);

    Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken);
    Task<string> ConfirmPhoneNumberAsync(string userId, string code);

    //Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    //Task<string> ResetPasswordAsync(ResetPasswordRequest request);
    //Task ChangePasswordAsync(ChangePasswordRequest request, string userId);
}

internal partial class UserService : IUserService
{
    #region
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IdentityContext _db;
    //private readonly IStringLocalizer _t;
    //private readonly IJobService _jobService;
    //private readonly IMailService _mailService;
    private readonly SecuritySettings _securitySettings;
    //private readonly IEmailTemplateService _templateService;
    //private readonly IFileStorageService _fileStorage;
    //private readonly IEventPublisher _events;
    private readonly ICacheService _cache;
    private readonly ICacheKeyService _cacheKeys;
    //private readonly ITenantInfo _currentTenant;

    public UserService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        //RoleManager<ApplicationRole> roleManager,
        IdentityContext db,
        //IStringLocalizer<UserService> localizer,
        //IJobService jobService,
        //IMailService mailService,
        //IEmailTemplateService templateService,
        //IFileStorageService fileStorage,
        //IEventPublisher events,
        ICacheService cache,
        ICacheKeyService cacheKeys,
        //ITenantInfo currentTenant,
        IOptions<SecuritySettings> securitySettings)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        //_roleManager = roleManager;
        _db = db;
        //_t = localizer;
        //_jobService = jobService;
        //_mailService = mailService;
        //_templateService = templateService;
        //_fileStorage = fileStorage;
        //_events = events;
        _cache = cache;
        _cacheKeys = cacheKeys;
        //_currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
    }
    #endregion

    #region confirm
    private async Task<string> GetEmailVerificationUriAsync(ApplicationUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/users/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        string verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), QueryStringKeys.UserId, user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, QueryStringKeys.Code, code);
        //verificationUri = QueryHelpers.AddQueryString(verificationUri, MultitenancyConstants.TenantIdName, _currentTenant.Id!);
        return verificationUri;
    }

    public async Task<string> ConfirmEmailAsync(string userId, string code, string tenant, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == userId && !u.EmailConfirmed)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new InternalServerException("An error occurred while confirming E-Mail.");

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);

        return result.Succeeded
            ? string.Format("Account Confirmed for E-Mail {0}. You can now use the /api/tokens endpoint to generate JWT.", user.Email)
            : throw new InternalServerException(string.Format("An error occurred while confirming {0}", user.Email));
    }

    public async Task<string> ConfirmPhoneNumberAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new InternalServerException("An error occurred while confirming Mobile Phone.");

        var result = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);

        return result.Succeeded
            ? user.EmailConfirmed
                ? string.Format("Account Confirmed for Phone Number {0}. You can now use the /api/tokens endpoint to generate JWT.", user.PhoneNumber)
                : string.Format("Account Confirmed for Phone Number {0}. You should confirm your E-mail before using the /api/tokens endpoint to generate JWT."
                , user.PhoneNumber)
            : throw new InternalServerException(string.Format("An error occurred while confirming {0}", user.PhoneNumber));
    }

    public Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region password
    #endregion

    #region permissions
    public async Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        _ = user ?? throw new UnauthorizedException("Authentication Failed.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var permissions = new List<string>();
        //foreach (var role in await _roleManager.Roles
        //    .Where(r => userRoles.Contains(r.Name))
        //    .ToListAsync(cancellationToken))
        //{
        //    permissions.AddRange(await _db.RoleClaims
        //        .Where(rc => rc.RoleId == role.Id && rc.ClaimType == TUFClaims.Permission)
        //        .Select(rc => rc.ClaimValue)
        //        .ToListAsync(cancellationToken));
        //}

        return permissions.Distinct().ToList();
    }

    public async Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken)
    {
        var permissions = await _cache.GetOrSetAsync(
            _cacheKeys.GetCacheKey(TUFClaims.Permission, userId),
            () => GetPermissionsAsync(userId, cancellationToken),
            cancellationToken: cancellationToken);

        return permissions?.Contains(permission) ?? false;
    }

    public Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken) =>
        _cache.RemoveAsync(_cacheKeys.GetCacheKey(TUFClaims.Permission, userId), cancellationToken);
    #endregion

    #region roles
    public async Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var user = await _userManager.FindByIdAsync(userId);
        //var roles = await _roleManager.Roles.AsNoTracking().ToListAsync(cancellationToken);
        //foreach (var role in roles)
        //{
        //    userRoles.Add(new UserRoleDto
        //    {
        //        RoleId = role.Id,
        //        RoleName = role.Name,
        //        Description = role.Description,
        //        Enabled = await _userManager.IsInRoleAsync(user, role.Name)
        //    });
        //}

        return userRoles;
    }

    public async Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new NotFoundException("User Not Found.");

        // Check if the user is an admin for which the admin role is getting disabled
        if (await _userManager.IsInRoleAsync(user, TUFRoles.Admin)
            && request.UserRoles.Any(a => !a.Enabled && a.RoleName == TUFRoles.Admin))
        {
            // Get count of users in Admin Role
            int adminCount = (await _userManager.GetUsersInRoleAsync(TUFRoles.Admin)).Count;

            // Check if user is not Root Tenant Admin
            // Edge Case : there are chances for other tenants to have users with the same email as that of Root Tenant Admin. Probably can add a check while User Registration
            //if (user.Email == MultitenancyConstants.Root.EmailAddress)
            //{
            //    if (_currentTenant.Id == MultitenancyConstants.Root.Id)
            //    {
            //        throw new ConflictException(_t["Cannot Remove Admin Role From Root Tenant Admin."]);
            //    }
            //}
            //else if (adminCount <= 2)
            //{
            //    throw new ConflictException(_t["Tenant should have at least 2 Admins."]);
            //}
        }

        //foreach (var userRole in request.UserRoles)
        //{
        //    // Check if Role Exists
        //    if (await _roleManager.FindByNameAsync(userRole.RoleName) is not null)
        //    {
        //        //if (userRole.Enabled)
        //        {
        //            if (!await _userManager.IsInRoleAsync(user, userRole.RoleName))
        //            {
        //                await _userManager.AddToRoleAsync(user, userRole.RoleName);
        //            }
        //        }
        //        else
        //        {
        //            await _userManager.RemoveFromRoleAsync(user, userRole.RoleName);
        //        }
        //    }
        //}

        //await _events.PublishAsync(new ApplicationUserUpdatedEvent(user.Id, true));

        return "User Roles Updated Successfully.";
    }

    public Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    //public Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}
    #endregion








}
