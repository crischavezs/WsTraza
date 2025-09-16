using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.DateTime;

namespace WsTraza.Middleware;

public class JwtManager
{
    public static string GenerateToken(string? username, string key, int tiempo)
    {
        var symmetricKey = Convert.FromBase64String(key);
        var tokenHandler = new JwtSecurityTokenHandler();

        var now = UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = now.AddMinutes(tiempo),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }

    public static bool ValidateToken(string token, string key)
    {
        var simplePrinciple = GetPrincipal(token, key);

        return simplePrinciple.Identity is ClaimsIdentity { IsAuthenticated: true } identity;
        // var usernameClaim = identity.FindFirst(ClaimTypes.Name);
        // More validate to check whether username exists in system
    }

    public static ClaimsPrincipal GetPrincipal(string token, string key)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(key);

            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch (SecurityTokenExpiredException ex)
        {
            throw new SecurityTokenExpiredException("El token ha expirado.", ex);
        }
        catch (SecurityTokenException ex)
        {
            throw new SecurityTokenException("Token inválido.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar el token.", ex);
        }
    }

}
