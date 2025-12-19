using Humanizer.Bytes;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using pleaseworkplease;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LiteraryArchiveServer
{
    public class JwtHandling(UserManager<Users> userManager, IConfiguration configuration)
    {
        public async Task<JwtSecurityToken> GenerateToken(Users user)
        {
            return new JwtSecurityToken
                (
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["JwtSettings:ExpiryMinutes"])),
                claims: await GetClaimsAsync(user),
                signingCredentials: GetSigningCredentials()
                );
        }
        private SigningCredentials GetSigningCredentials() 
        {
            byte[] key = Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!);
            SymmetricSecurityKey signingkey = new(key);
            return new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaimsAsync(Users user)
        {
            List<Claim> claims = [new Claim(ClaimTypes.Name, user.UserName!)];
            foreach (var role in await userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
