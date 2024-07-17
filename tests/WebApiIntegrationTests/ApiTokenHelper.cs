using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingApp.ApplicationCore.Constants;

namespace TicketingApp.WebApiIntegrationTests;

public class ApiTokenHelper
{
    public static string GetAdminUserToken()
    {
        string userName = "admin@ticketing.com";
        string[] roles = { AuthorizationConstants.ADMINISTRATORS };

        return CreateToken(userName, roles);
    }

    public static string GetCustomerUserToken()
    {
        string userName = "customer@ticketing.com";
        string[] roles = { AuthorizationConstants.CUSTOMERS };

        return CreateToken(userName, roles);
    }

    private static string CreateToken(string userName, string[] roles)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = Encoding.ASCII.GetBytes(AuthorizationConstants.JWT_SECRET_KEY);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
