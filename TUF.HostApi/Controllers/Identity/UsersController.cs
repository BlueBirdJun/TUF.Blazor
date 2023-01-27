using Knus.Common.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using NSwag.Annotations;
using TUF.Database.Identity.Models;
using TUF.Infrastructure.Auth.Permissions;
using TUF.Infrastructure.Identity.Users;
using TUF.Shared.Dtos;
using TUF.Shared.Dtos.Member;

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
        
        //비번찾기


        [HttpPost]
        [Route("Resister")]
        [AllowAnonymous]
        public async Task<CreateUserDto> ResisterUser([FromBody] CreateUserDto.Request data)
        {
            //DtoBase<string, string> rt = new DtoBase<string, string>();
            CreateUserDto rt = new CreateUserDto();
            try
            {
                var fid = await _userManager.FindByNameAsync(data.UserName);
                if(fid != null)
                {
                    rt.Success = false;
                    rt.Message = "이미 등록된 아이디입니다.";
                    return rt;
                }
                var femail = await _userManager.FindByEmailAsync(data.UserEmail);
                if (femail != null)
                {
                    rt.Success = false;
                    rt.Message = "이미 있는 매일입니다.";
                    return rt;
                }

                var user = new ApplicationUser
                {
                    UserName = data.UserID,
                    Email = data.UserEmail,
                    FirstName = "",
                    LastName = data.UserName,// data.useremail,
                    MemberType = "MP",
                    JoinChanel = "N",
                    NickName = "",
                    CreateDate = DateTime.Now,
                    BlackMessage = "a",
                    CompanyName = "",
                    CompanyNumberAutoryn = "N",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    IsActive = true,
                };
                var result = await _userManager.CreateAsync(user, data.Password);
                if (result.Succeeded)
                {
                    rt.Success = true;
                }
                else
                {
                    
                    if (result.Errors.Any(p => p.Code == "DuplicateUserName"))
                        rt.Message += "이미 가입됬어\r\n";
                    if (result.Errors.Any(p => p.Code == "PasswordTooShort"))
                        rt.Message += "비밀번호가 좀 짧다\r\n";

                    if (rt.Message.IsNullOrEmpty())
                        rt.Message = "회원가입실패!";
                    rt.Success= false;                    
                }
                 
            }
            catch (Exception ex) {
                rt.Success= false;
                rt.Message = ex.Message;
            }
            return rt;
        }
    }

}
