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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool IsActive { get; set; } = false;
        [MaxLength(20)]
        public string MemberType { get; set; }
        [MaxLength(20)]
        public string JoinChanel { get; set; }
       

        [MaxLength(300)]
        public string NickName { get; set; }
        [MaxLength(300)]
        public string CompanyName { get; set; }

        [MaxLength(30)]
        public string CompanyNumberAutoryn { get; set; }
        [MaxLength(30)]
        public string UserPhoneNumberYn { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime FireDate { get; set; }

        public DateTime BlockDate { get; set; }
        public string BlackMessage { get; set; }
    }
}
