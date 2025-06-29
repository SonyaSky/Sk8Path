using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Spot : Point
    {
        public string AuthorId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid? FileId { get; set; }
        public User Author { get; set; }
        public int RatingSum { get; set; }
        public bool IsToBeDeleted { get; set; } = false;
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public List<FavoriteSpot> FavoritedByUsers { get; set; } = new List<FavoriteSpot>();
    }
}