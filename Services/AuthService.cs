using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace jwt_net;

public class AuthService :IAuthService
{
    string path = "./users.json";
    private readonly JwtSettings _jwtSettings;
    public AuthService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public async Task<string> GetJWT(string user, string password)
    {
        string content = await File.ReadAllTextAsync(path);
        List<User> users = JsonSerializer.Deserialize<List<User>>(content);
        User userFound = users.Find(p => p.user == user);

        if(userFound != null)
        {
            if(userFound.user == user && userFound.password == password)
            {
                return GenerateJwtToken(user) ;
                
            }else
            {
                return "usuario invalido";
            }
        }
        return "Usuario no existe";
    }
    private string GenerateJwtToken(string user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var byteKey = Encoding.UTF8.GetBytes(_jwtSettings.key);
        var tokenDes = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user),

            }),
            Expires = DateTime.UtcNow.AddMonths(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(byteKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDes);
        return tokenHandler.WriteToken(token);
    }
}

public interface IAuthService
{
    public Task<string> GetJWT(string user, string password);
}
