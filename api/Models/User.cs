using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public Guid? AvatarId { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}