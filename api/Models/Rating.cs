using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Rating
    {
        public string UserId { get; set; }
        public Guid ObjectId { get; set; }
        public int Score { get; set; }
    }
}