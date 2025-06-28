using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Roads;
using api.Models;

namespace api.Mappers
{
    public static class RoadMapper
    {
        public static RoadPointDto ToDto(this RoadPoint point)
        {
            return new RoadPointDto
            {
                Id = point.Id,
                Type = point.Type,
                ParentPointId = point.ParentPointId,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
            };
        }

        public static RoadPoint ToRoadPoint(this CreateRoadPointDto point)
        {
            return new RoadPoint
            {
                Id = Guid.NewGuid(),
                Type = point.Type,
                Latitude = point.Latitude,
                Longitude = point.Longitude,
            };
        }

        public static RoadDto ToDto(this Road road)
        {
            return new RoadDto
            {
                Id = road.Id,
                AuthorId = road.AuthorId,
                Rating = road.Ratings.Count == 0 ? road.RatingSum : road.RatingSum / road.Ratings.Count,
                Points = road.Points.Select(p => p.ToDto()).ToList()
            };
        }
    }
}