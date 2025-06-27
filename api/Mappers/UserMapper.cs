using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Models;

namespace api.Mappers
{
    public static class UserMapper
    {
        public static ProfileDto ToProfileDto(this User user)
        {
            return new ProfileDto
            {
                Id = new Guid(user.Id),
                Email = user.Email,
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public static User ToUser(this RegisterUserDto dto)
        {
            return new User
            {
                Email = dto.Email,
                UserName = dto.Username,
                PhoneNumber = dto.PhoneNumber,
            };
        }
    }
}