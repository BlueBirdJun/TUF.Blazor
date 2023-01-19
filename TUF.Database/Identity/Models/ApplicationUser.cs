using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Database.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; } = default;
        public string? LastName { get; set; } = default;
        //public byte[] ProfilePicture { get; set; } = default;
        public bool? IsActive { get; set; } = false;
        [MaxLength(20)]
        public string? MemberType { get; set; } = default;
        [MaxLength(20)]
        public string? JoinChanel { get; set; } = default;


        [MaxLength(300)]
        public string? NickName { get; set; } = default;
        [MaxLength(300)]
        public string? CompanyName { get; set; } = default;

        [MaxLength(30)]
        public string? CompanyNumberAutoryn { get; set; } = default;
        [MaxLength(30)]
        public string? PhoneNumber { get; set; } = default;

        [MaxLength(30)]
        public string? UserPhoneNumberYn { get; set; } = default;

        public DateTime CreateDate { get; set; }
        public DateTime?  FireDate { get; set; }

        public DateTime? BlockDate { get; set; }
        public string? BlackMessage { get; set; } = default;

        [NotMapped]
        public string? strProfilePicture { get; set; } = default;
        [NotMapped]
        public string? RefreshToken { get; set; } = default;
        [NotMapped]
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
