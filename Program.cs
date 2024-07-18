using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

string key = "svbjklkw3324km34klmfgl4fme4flkmeb";

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(opt => 
{
    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature);
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signingKey,
    };
});
var app = builder.Build();
app.MapGet("/", () => "hello");
app.MapGet("/protected", () => "hello")
    .RequireAuthorization();
app.MapGet("/auth/{user}/{pass}",(string user, string pass) => 
{
    if(user == "pato" && pass == "donald")
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var byteKey = Encoding.UTF8.GetBytes(key);
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
    else
    {
        return "usuario invalido";
    }
});

app.Run();
