using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    /// <summary>
    /// This class contains the method that generate JWT Tokens
    /// </summary>
    public static class TokenService
    {
        /// <summary>
        /// Generates an token for an specific <see cref="User"/>
        /// </summary>
        /// <param name="user"><see cref="User"/> to generate the token</param>
        /// <returns> A string representing the token for the user</returns>
        public static string generateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Utils.SECRET);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role.ToString())
                }),

                Expires = DateTime.UtcNow.AddMinutes(Utils.TOKEN_DURATION),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}