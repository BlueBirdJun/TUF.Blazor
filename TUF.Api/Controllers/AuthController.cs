using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TUF.Shared.Dtos.Member;
using TUF.Shared.Dtos;
using TUF.Domains.Entities;
using Microsoft.AspNetCore.Identity;
using TUF.Database.Identity.Models;

namespace TUF.Api.Controllers
{
    
    public class AuthController : VersionNeutralApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_cookieAuthContext = cookieAuthContext;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginDto login)
        {
            //var user = await _cookieAuthContext
            //.User.Where(_ => _.Email.ToLower() == login.Email.ToLower() &&
            //_.Password == login.Password && _.ExternalLoginName == null)
            //.FirstOrDefaultAsync();
            var user = new User();
            user.Email = login.Email;
            user.FirstName = "김";
            user.LastName = "광일";
            user.Id = 2;
            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }
            var claims = new List<Claim>
                {
                    new Claim("userid", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(TUFClaims.Tenant, "Tenant_1"),
                    new Claim(TUFClaims.Fullname, "Tenant_1"),
                    new Claim(TUFClaims.Permission, "Tenant_1"),
                    new Claim(TUFClaims.ImageUrl, "Tenant_1"),
                    new Claim(TUFClaims.IpAddress, "Tenant_1"),
                    new Claim(TUFClaims.Expiration, "2022-02-02")
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok("Success");
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("success");
        }

        [Authorize]
        [HttpGet]
        [Route("user-profile")]
        public async Task<IActionResult> UserProfileAsync()
        {
            int userId = HttpContext.User.Claims
            .Where(_ => _.Type == "userid")
            .Select(_ => Convert.ToInt32(_.Value))
            .First();

            //var userProfile = await _cookieAuthContext
            //.User
            //.Where(_ => _.Id == userId)
            //.Select(_ => new UserProfileDto
            //{
            //    UserId = _.Id,
            //    Email = _.Email,
            //    FirstName = _.FirstName,
            //    LastName = _.LastName
            //}).FirstOrDefaultAsync();
            var userProfile = new UserProfileDto();
            userProfile.FirstName = "김";
            userProfile.LastName = "광일";
            userProfile.Email = "kki2020@dam.net";
            userProfile.UserId = 2;
            return Ok(userProfile);
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> GetUser()
        {
            var c = await _userManager.FindByEmailAsync("");
            //await HttpContext.SignOutAsync();
            return Ok("success");
        }


        [HttpPost]
        [Route("usercreate")]
        public async Task<IActionResult> AddUser(string email)
        {
            var user = new ApplicationUser
            {
                UserName = "김광일",
                Email = email,
                FirstName = "",
                LastName = " ",// data.useremail,
                MemberType = "MP",
                JoinChanel = "N",
                NickName = "별명",
                CreateDate = DateTime.Now,
                BlackMessage = "a",
                CompanyName = "회사이름",
                CompanyNumberAutoryn = "N",
                EmailConfirmed = true,
                PhoneNumberConfirmed= true,
                TwoFactorEnabled= true,                
            };
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


    }
}
