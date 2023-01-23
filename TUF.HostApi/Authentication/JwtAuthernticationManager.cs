using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TUF.HostApi.Authentication;

public class JwtAuthernticationManager
{
    public const string JWT_SECURITY_KEY = "S0M3RAN0MS3CR3T!1!MAG1C!1!";
    public int JWT_TOKEN_VALIDITY_MINS = 20;

    public UserSession? GenerateJwtToken(string username, string password)
    {
        //if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        //    return null;
        //var userAccount = _userAccountService.GetUserAccountByUserName(username);
        //if (userAccount == null || userAccount.Password != password)
        //    return null;

        var toeknExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var clamimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name,"kim kwangil"),
            new Claim(ClaimTypes.Role,"Administrator"),
        });
        var signingCredentitals = new SigningCredentials
        (new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = clamimsIdentity,
            Expires = toeknExpiryTimeStamp,
            SigningCredentials = signingCredentitals
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToekn = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToekn);
        var userSession = new UserSession
        {
            UserName = "kim kwangil",
            Role = "Administrator",
            Token = token,
            ExpiresIn = (int)toeknExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
        };
        return userSession;
    }

}

