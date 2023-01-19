using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knus.Common.Models
{
    public class BaseJsonDto<T>
    {
        public bool Success { get; set; } = default;
        public bool hasError { get; set; } = default;
        public bool hasAlert { get; set; } = default;
        public string Message { get; set; } = default;
        public string html { get; set; } = default;
        public T Result { get; set; } 
        public int TotalCount { get; set; } = default;
    }
}
