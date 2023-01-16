using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.Common.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.Now;
    }
}
