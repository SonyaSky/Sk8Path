using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Dtos;
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

        public async Task<ResponseModel?> AddToFavourites(Guid id, string userId)
        {
            var spot = await _context.Spots.FirstOrDefaultAsync(s => s.Id == id);
            if (spot == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Spot with id={id} not found"
                };
            }
            var exists = await _context.FavoriteSpots
                .AnyAsync(f => f.UserId == userId && f.SpotId == id);
            if (!exists)
            {
                var fav = new FavoriteSpot
                {
                    UserId = userId,
                    SpotId = id
                };
                _context.FavoriteSpots.Add(fav);
                await _context.SaveChangesAsync();
                return null;
            }
            return new ResponseModel
            {
                Status = "Error: BadRequest",
                Message = $"Spot with id={id} already added to favorites"
            };
        }

        public async Task<ResponseModel?> ApproveDeleting(Guid id)
        {
            var spot = await _context.Spots.FirstOrDefaultAsync(x => x.Id == id);
            if (spot == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Spot with id={id} not found"
                };
            }
            var ratings = await _context.Ratings.Where(x => x.ObjectId == spot.Id).ToListAsync();
            _context.Ratings.RemoveRange(ratings);
            _context.Spots.Remove(spot);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<SpotDto> CreateSpot(CreateSpotDto spotDto, string userId)
        {
            var spot = new Spot
            {
                Id = Guid.NewGuid(),
                AuthorId = userId,
                RatingSum = spotDto.Rating,
                Latitude = spotDto.Latitude,
                Longitude = spotDto.Longitude,
                CreateDate = DateTime.UtcNow,
                Name = spotDto.Name,
                Description = spotDto.Description,
                FileId = spotDto.FileId
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

        public async Task<ResponseModel?> DeclineDeleting(Guid id)
        {
            var spot = await _context.Spots.FirstOrDefaultAsync(x => x.Id == id);
            if (spot == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Spot with id={id} not found"
                };
            }
            spot.IsToBeDeleted = false;
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<List<SpotDto>> GetAllSpots()
        {
            var spots = await _context.Spots.Include(x => x.Ratings).Select(x => x.ToDto()).ToListAsync();
            return spots;
        }

        public async Task<List<SpotDto>> GetMySpots(string userId)
        {
            var spots = await _context.Spots
                .Include(x => x.Ratings)
                .Where(x => x.AuthorId == userId)
                .Select(x => x.ToDto())
                .ToListAsync();
            return spots;
        }

        public async Task<ResponseModel?> RateSpot(CreateRatingDto ratingDto, string userId)
        {
            var spot = await _context.Spots.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == ratingDto.ObjectId);
            if (spot == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Spot with id={ratingDto.ObjectId} not found"
                };
            }
            var isAlreadyRated = await _context.Ratings.FirstOrDefaultAsync(x => x.ObjectId == ratingDto.ObjectId && x.UserId == userId);
            if (isAlreadyRated != null)
            {
                spot.RatingSum -= isAlreadyRated.Score;
                spot.RatingSum += ratingDto.Rating;
                isAlreadyRated.Score = ratingDto.Rating;
                var oldRating = spot.Ratings.FirstOrDefault(x => x.ObjectId == ratingDto.ObjectId);
                oldRating.Score = ratingDto.Rating;
            }
            else
            {
                var rating = new Rating
                {
                    UserId = userId,
                    ObjectId = spot.Id,
                    Score = ratingDto.Rating
                };
                await _context.Ratings.AddAsync(rating);
                spot.RatingSum += ratingDto.Rating;
                spot.Ratings.Add(rating);
            }
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<ResponseModel?> RemoveFromFavourites(Guid id, string userId)
        {
            var favorite = await _context.FavoriteSpots
                .FirstOrDefaultAsync(f => f.UserId == userId && f.SpotId == id);

            if (favorite == null)
                return new ResponseModel
                {
                    Status = "Error: BadRequest",
                    Message = $"Spot with id={id} isn't in your favorites"
                };

            _context.FavoriteSpots.Remove(favorite);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<ResponseModel?> SendRequestToDelete(Guid id)
        {
            var spot = await _context.Spots.FirstOrDefaultAsync(x => x.Id == id);
            if (spot == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Spot with id={id} not found"
                };
            }
            spot.IsToBeDeleted = true;
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<List<SpotDto>> ShowFavouriteSpots(string userId)
        {
            var favoriteSpots = await _context.FavoriteSpots
                .Where(f => f.UserId == userId)
                .Include(f => f.Spot)
                    .ThenInclude(s => s.Ratings)
                .Select(f => f.Spot.ToDto())
                .ToListAsync();

            return favoriteSpots;
        }

        public async Task<List<SpotDto>> ShowToDeleteSpots()
        {
            return await _context.Spots
                .Include(x => x.Ratings)
                .Where(x => x.IsToBeDeleted)
                .Select(x => x.ToDto())
                .ToListAsync();
        }
    }
}