using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class TokenData
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? TokenType { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}