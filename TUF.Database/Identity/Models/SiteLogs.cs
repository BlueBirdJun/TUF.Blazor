using Daniel.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Database.Identity.Models
{
    public class SiteLogs: AudiTableEntity
    {
        
        public string UserId { get; set; }
        public string Kind { get; set; }
        public string Memo { get; set; }
    }
}
