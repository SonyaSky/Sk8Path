using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse?> Login(LoginDto loginDto);
        Task<TokenResponse?> RefreshToken(RefreshDto refreshDto);
        Task<ResponseModel> Logout(string id);
        // Task<TokenResponse?> ChangeLoginCredentials(ChangeLoginDto loginDto, string id);
    }
}