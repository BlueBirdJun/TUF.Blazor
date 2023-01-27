using Daniel.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos.Member;

public class CreateUserDto :DtoBase<CreateUserDto.Response,CreateUserDto.Request>
{    
    public class Request 
    {
        public readonly ApiMetaData metadata=new ApiMetaData
        {
            Title = "회원가입",
            httpmethod = Knus.Common.Services.HttpMethods.POST,
            UrlPath = "/api/users/Resister"
        };
        public Request()
        {
           
        }


        [Newtonsoft.Json.JsonProperty("UserID", Required = Newtonsoft.Json.Required.Always)]
        [Required(ErrorMessage = "아이디 필수")]
        public string UserID { get; set; }
        [Newtonsoft.Json.JsonProperty("UserName", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]

        public string UserName { get; set; }
        [Newtonsoft.Json.JsonProperty("UserEmail", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[^@]+@[^@]+$", ErrorMessage = "이메일 단디 안적나")]
        public string UserEmail { get; set; }
        [Newtonsoft.Json.JsonProperty("Password", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; }
        [Newtonsoft.Json.JsonProperty("ConfirmPassword", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required]
        public string ConfirmPassword { get; set; } = default!;

        [Newtonsoft.Json.JsonProperty("phoneNumber", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string? PhoneNumber { get; set; } = default!;
    }
    public class Response
    {
        public string UserName { get; set; }
    }
}
