using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public User User { get; set; }
    }
}