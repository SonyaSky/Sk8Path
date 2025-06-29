using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using api.Validations;

namespace api.Dtos
{
    [AtLeastOneContactRequired("Email", "PhoneNumber")]
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        [PhoneNumber]
        public string? PhoneNumber { get; set; }
        public Guid? FileId { get; set; }
    }
}