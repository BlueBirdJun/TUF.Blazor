using Daniel.Common.Interfaces;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TUF.Database.DbContexts;
using TUF.Database.Identity.Models;
using TUF.Infrastructure.Auth;

namespace TUF.Application.Identity.Users;

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
/*
internal partial class UserService : IUserService
{
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
    //private readonly ICacheService _cache;
    //private readonly ICacheKeyService _cacheKeys;
    //private readonly ITenantInfo _currentTenant;

    public UserService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IdentityContext db,
        IStringLocalizer<UserService> localizer,
        //IJobService jobService,
        //IMailService mailService,
        //IEmailTemplateService templateService,
        //IFileStorageService fileStorage,
        //IEventPublisher events,
        //ICacheService cache,
        //ICacheKeyService cacheKeys,
        ITenantInfo currentTenant,
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
        //_cache = cache;
        //_cacheKeys = cacheKeys;
        //_currentTenant = currentTenant;
        _securitySettings = securitySettings.Value;
    }
}
*/