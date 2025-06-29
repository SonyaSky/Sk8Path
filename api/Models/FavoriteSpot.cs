using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class FavoriteSpot
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid SpotId { get; set; }
        public Spot Spot { get; set; }
    }
}