using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos
{
    public class LoginDto
    {
        
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.RegularExpression(@"^[^@]+@[^@]+$")]
        public string Email { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; }
    }
}
