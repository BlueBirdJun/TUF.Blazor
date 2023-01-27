using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Shared.Dtos.Member;

namespace TUF.Shared.Dtos
{
    public class LoginDto : DtoBase<LoginDto.Response, LoginDto.Request>
    {

        public class Request {
            public readonly ApiMetaData metadata = new ApiMetaData
            {
                Title = "로그인",
                httpmethod = Knus.Common.Services.HttpMethods.POST,
                UrlPath = "/api/tokens/Login"
            };
            
            [Required(ErrorMessage = "아이디 필수")]
            public string UserId { get; set; }

            [Required(ErrorMessage = "비번 필수")]
            public string Password { get; set; }
        }
        public record Response (string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);

    }
}
