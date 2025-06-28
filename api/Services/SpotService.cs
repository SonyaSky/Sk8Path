using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Dtos.Spots;
using api.Interfaces;
using api.Mappers;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class SpotService : ISpotService
    {
        private readonly ApplicationDBContext _context;
        public SpotService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<SpotDto> CreateSpot(CreateSpotDto spotDto, string userId)
        {
            var spot = new Spot
            {
                Id = new Guid(),
                AuthorId = userId,
                RatingSum = spotDto.Rating,
                Latitude = spotDto.Latitude,
                Longitude = spotDto.Longitude,
                CreateDate = DateTime.UtcNow
            };
            var rating = new Rating
            {
                UserId = userId,
                ObjectId = spot.Id,
                Score = spotDto.Rating
            };
            spot.Ratings.Add(rating);
            await _context.Ratings.AddAsync(rating);
            await _context.Spots.AddAsync(spot);
            await _context.SaveChangesAsync();
            return spot.ToDto();
        }

        public async Task<List<SpotDto>> GetAllSpots()
        {
            var spots = await _context.Spots.Include(x => x.Ratings).Select(x => x.ToDto()).ToListAsync();
            return spots;
        }
    }
}