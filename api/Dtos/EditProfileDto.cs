using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using api.Validations;

namespace api.Dtos
{
    [AtLeastOneContactRequired("Email", "PhoneNumber")]
    public class EditProfileDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MaxLength(1000, ErrorMessage = " The field Username must be a string or array type with a maximum length of '1000'.")]
        public string Username { get; set; } = string.Empty;
        [PhoneNumber]
        public string? PhoneNumber { get; set; }
        public Guid? AvatarId { get; set; }
    }
}