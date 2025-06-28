using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Spots;
using api.Models;

namespace api.Mappers
{
    public static class SpotMapper
    {
        public static SpotDto ToDto(this Spot spot)
        {
            return new SpotDto
            {
                Id = spot.Id,
                Latitude = spot.Latitude,
                Longitude = spot.Longitude,
                CreateDate = spot.CreateDate,
                AuthorId = spot.AuthorId,
                Rating = spot.Ratings.Count == 0
                    ? 0
                    : (double)spot.RatingSum / spot.Ratings.Count
            };
        }
    }
}