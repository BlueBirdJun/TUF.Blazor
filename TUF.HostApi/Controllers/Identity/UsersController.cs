using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using NSwag.Annotations;
using TUF.Database.Identity.Models;
using TUF.Infrastructure.Auth.Permissions;
using TUF.Infrastructure.Identity.Users;

namespace TUF.HostApi.Controllers.Identity
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : VersionNeutralApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(UserManager<ApplicationUser> userService)
        {
            _userManager = userService;
        }

        [HttpPost]
        [Route("usercreate")]
        public async Task<IActionResult> AddUser(string email)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstName = "kim",
                LastName = "wangil",// data.useremail,
                MemberType = "MP",
                JoinChanel = "N",
                NickName = "별명",
                CreateDate = DateTime.Now,
                BlackMessage = "a",
                CompanyName = "회사이름",
                CompanyNumberAutoryn = "N",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = true,
                IsActive = true,

            };
            try
            {
                var r1 = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.CreateAsync(user, "abcde123");
                if (result.Succeeded)
                {
                    return Ok("success");
                }
                else
                {
                    return Ok("fail");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }  
            return Ok("fail");
        }
    }

}
