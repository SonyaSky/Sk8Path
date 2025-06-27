using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(string id, IList<string> roles, DateTime expirationDate, string type);
        string? ValidateToken(string token, string type);
        Task<string> CreateRefreshToken(string id, IList<string> roles);
        Task<string> CreateAccessToken(string id, IList<string> roles);
        bool ValidateRefreshToken(string token);
        bool ValidateAccessToken(string token);
        TokenData? GetTokenData(string token);
    }
}