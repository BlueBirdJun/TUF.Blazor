using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos
{
    public class LoginDto
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
        public class Response {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public DateTime RefreshTokenExpiryTime { get; set; }            
        } 
    }
}
