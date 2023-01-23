namespace TUF.HostApi.Authentication;

public class UserSession
{
    public UserSession() { }
    public string UserName { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public int ExpiresIn { get; set; }
    public DateTime ExpriyTimeStamp { get; set; }
}
