using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<User> _userManager;
        public TokenService(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
            _userManager = userManager;
        }

        public async Task<string> CreateAccessToken(string id, IList<string> roles)
        {
            return await CreateToken(id, roles, DateTime.Now.AddMinutes(60), "AccessToken");
        }

        public async Task<string> CreateRefreshToken(string id, IList<string> roles)
        {
            var token = await CreateToken(id, roles, DateTime.Now.AddDays(2), "RefreshToken");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            user.RefreshToken = token;
            await _userManager.UpdateAsync(user);
            return token;
        }

        public async Task<string> CreateToken(string id, IList<string> roles, DateTime expirationDate, string type)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim("TokenType", type)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateRefreshToken(string token)
        {
            var result = ValidateToken(token, "RefreshToken");
            if (result == null) return false;

            var data = GetTokenData(token);
            if (data == null || data.Id == null) return false;

            var user = _userManager.Users.FirstOrDefault(u => u.Id == data.Id);
            if (user == null || user.RefreshToken != token) return false;

            return true; 
        }

        public bool ValidateAccessToken(string token)
        {
            var result = ValidateToken(token, "AccessToken");
            if (result == null) return false;
            return true;
        }

        public string? ValidateToken(string token, string type)
        {
            if (token.IsNullOrEmpty()) return null;
            var tokenData = GetTokenData(token);
            if (tokenData != null)
            {
                if (tokenData.TokenType == null || tokenData.TokenType != type) {
                    return null;
                }
                return tokenData.Id;
            }
            return null;
        }

        public TokenData? GetTokenData(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
                if (principal != null && principal.Identity.IsAuthenticated) {
                return new TokenData {
                    Id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    TokenType = principal.FindFirst("TokenType")?.Value,
                    Roles = principal.FindAll(ClaimTypes.Role).Select(role => role.Value).ToList()
                };
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null; 
            }
            return null;
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            var applicant = await _userManager.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (applicant == null) return false;
            return true;
        }
    }
}