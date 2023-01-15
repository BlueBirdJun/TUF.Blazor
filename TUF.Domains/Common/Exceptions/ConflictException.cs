using System.Net;

namespace TUF.Domains.Common.Exceptions;
public class ConflictException : CustomException
{
    public ConflictException(string message)
        : base(message, null, HttpStatusCode.Conflict)
    {
    }
}