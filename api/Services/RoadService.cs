using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Dtos.Roads;
using api.Interfaces;
using api.Mappers;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class RoadService : IRoadService
    {
        private readonly ApplicationDBContext _context;
        public RoadService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<RoadDto> CreateRoad(CreateRoadDto roadDto, string userId)
        {
            var road = new Road
            {
                Id = Guid.NewGuid(),
                AuthorId = userId,
                RatingSum = roadDto.Rating
            };
            var rating = new Rating
            {
                UserId = userId,
                ObjectId = road.Id,
                Score = roadDto.Rating
            };
            road.Ratings.Add(rating);
            foreach (CreateRoadPointDto pointDto in roadDto.Points)
            {
                var point = pointDto.ToRoadPoint();
                if (road.Points.Count != 0)
                {
                    point.ParentPoint = road.Points[road.Points.Count - 1];
                    point.ParentPointId = road.Points[road.Points.Count - 1].Id;
                }
                else
                {
                    point.ParentPoint = null;
                    point.ParentPointId = null;
                }
                road.Points.Add(point);
            }
            await _context.Roads.AddAsync(road);
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
            return road.ToDto();
        }

        public async Task<List<RoadDto>> GetAllRoads()
        {
            var roads = await _context.Roads
                .Include(x => x.Points)
                .Include(x => x.Ratings)
                .Select(x => x.ToDto())
                .ToListAsync();
            return roads;
        }
    }
}