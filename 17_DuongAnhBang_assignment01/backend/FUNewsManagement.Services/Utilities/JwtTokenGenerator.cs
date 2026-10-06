using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FUNewsManagement.Services.Utilities
{
    public sealed class JwtTokenGenerator
    {
        private static JwtTokenGenerator _instance = null;
        private static readonly object _lock = new object();

        public static JwtTokenGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new JwtTokenGenerator();
                        }
                    }
                }
                return _instance;
            }
        }

        private JwtTokenGenerator() { }

        public string GenerateToken(string email, string role, string name, short? accountId, string secretKey, string issuer, string audience, int expireMinutes = 120)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, name ?? email),
                new Claim("AccountID", accountId.HasValue ? accountId.Value.ToString() : "0"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
