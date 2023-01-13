using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knus.Common.Models
{
    public class BaseJsonDto<T>
    {
        public bool Success { get; set; }
        public bool hasError { get; set; }
        public bool hasAlert { get; set; }
        public string Message { get; set; }
        public string html { get; set; }
        public T Result { get; set; }
        public int TotalCount { get; set; }
    }
}
