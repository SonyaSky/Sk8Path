using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class LoginDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "The Email field is required.")]
        public required string Username { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "The Password field is required.")]
        public required string Password { get; set; }
    }
}