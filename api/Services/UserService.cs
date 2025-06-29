using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Interfaces;
using api.Mappers;
using api.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ProfileDto?> EditUserProfile(string userId, EditProfileDto profileDto)
        {
            var user = await _userManager.Users
                .OfType<User>()
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (user.UserName != profileDto.Username)
            {
                if (await IsUsernameTaken(profileDto.Username))
                {
                    return null;
                }
            }

            user.Email = profileDto.Email;
            user.UserName = profileDto.Username;
            user.PhoneNumber = profileDto.PhoneNumber;
            user.AvatarId = profileDto.AvatarId;

            await _userManager.UpdateAsync(user);
            return user.ToProfileDto();
        }

        public async Task<ProfileDto?> GetUserProfile(string id)
        {
            var user = await _userManager.Users
                .OfType<User>()
                .FirstOrDefaultAsync(a => a.Id == id);
            if (user == null) return null;
            return user.ToProfileDto();
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            return _userManager.Users.Any(u => u.UserName == username);
        }

        public async Task<TokenResponse?> RegisterUser(RegisterUserDto registerDto)
        {
            var user = registerDto.ToUser();
            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (roleResult.Succeeded)
                {
                    return new TokenResponse
                    {
                        AccessToken = await _tokenService.CreateAccessToken(user.Id, ["User"]),
                        RefreshToken = await _tokenService.CreateRefreshToken(user.Id, ["User"]),
                    };
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}