using System.Net;

namespace TUF.Domains.Common.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, HttpStatusCode.NotFound)
    {
    }
}