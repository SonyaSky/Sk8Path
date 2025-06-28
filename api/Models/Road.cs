using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Road
    {
        public Guid Id { get; set; }
        public string AuthorId { get; set; }
        public User Author { get; set; }
        public int RatingSum { get; set; }
        public List<RoadPoint> Points { get; set; } = new List<RoadPoint>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}