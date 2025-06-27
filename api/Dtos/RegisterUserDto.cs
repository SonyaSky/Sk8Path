using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.Validations;

namespace api.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Username{ get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [PhoneNumber]
        public string? PhoneNumber { get; set; } = string.Empty;
    }
}