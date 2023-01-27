using Knus.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daniel.Common.Models;

public class ApiMetaData
{
    public string UrlPath { get; set; }
    public HttpMethods httpmethod { get; set; }
    public string Title { get; set; }
}
