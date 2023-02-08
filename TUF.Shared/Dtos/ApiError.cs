using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Dtos;

public enum ApiEnum
{
    UnAuthorized, OnForbidden,NotFound
}
public class ApiError
{
    public ApiEnum ApiEnum { get; set; }
    public string Message { get; set; }
}
