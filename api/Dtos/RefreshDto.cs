using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class RefreshDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}