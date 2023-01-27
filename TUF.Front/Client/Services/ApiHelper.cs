using System.Net.Http.Headers;

namespace TUF.Front.Client.Services
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
