using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Spot : Point
    {
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public int RatingSum { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}