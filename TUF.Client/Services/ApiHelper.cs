using System.Net.Http.Headers;

namespace TUF.Client.Services
{
    public static class ApiHelper
    {
        public static AuthenticationHeaderValue GetHeader(string token)
        {
            AuthenticationHeaderValue header = new AuthenticationHeaderValue("bearer", token); 
            return new AuthenticationHeaderValue(token);
        }
    }
}
