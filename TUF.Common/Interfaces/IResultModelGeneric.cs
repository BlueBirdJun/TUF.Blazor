

using Daniel.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knus.Common.Interfaces
{
    public interface IResultModelGeneric<T, S>
    {
        bool Success { get; set; }
        string Code { get; set; }
        bool HasAlert { get; set; }
        bool HasError { get; set; }
        S InputValue { get; set; }
        string Message { get; set; }
        List<string> ListMessage { get; set; }
        T OutPutValue { get; set; }

        ContentState ContentState { get; set; }
    }
}
