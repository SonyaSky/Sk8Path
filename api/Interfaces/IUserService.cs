using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Interfaces
{
    public interface IUserService
    {
        Task<ProfileDto?> GetUserProfile(string id);
        Task<TokenResponse?> RegisterUser(RegisterUserDto registerDto);
        Task<ProfileDto?> EditUserProfile(string userId, EditProfileDto profileDto);
        Task<bool> IsUsernameTaken(string username);
    }
}