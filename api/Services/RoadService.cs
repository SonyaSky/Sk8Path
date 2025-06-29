using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Dtos;
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

        public async Task<ResponseModel?> ApproveDeleting(Guid id)
        {
            var road = await _context.Roads.FirstOrDefaultAsync(x => x.Id == id);
            if (road == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Road with id={id} not found"
                };
            }
            var ratings = await _context.Ratings.Where(x => x.ObjectId == road.Id).ToListAsync();
            _context.Ratings.RemoveRange(ratings);
            var roadPoints = await _context.RoadPoints.Where(x => x.RoadId == road.Id).ToListAsync();
            _context.RoadPoints.RemoveRange(roadPoints);
            _context.Roads.Remove(road);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<RoadDto> CreateRoad(CreateRoadDto roadDto, string userId)
        {
            var road = new Road
            {
                Id = Guid.NewGuid(),
                AuthorId = userId,
                RatingSum = roadDto.Rating,
                Name = roadDto.Name,
                Description = roadDto.Description,
                FileId = roadDto.FileId
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

        public async Task<ResponseModel?> DeclineDeleting(Guid id)
        {
            var road = await _context.Roads.FirstOrDefaultAsync(x => x.Id == id);
            if (road == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Road with id={id} not found"
                };
            }
            road.IsToBeDeleted = false;
            await _context.SaveChangesAsync();
            return null;
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

        public async Task<List<RoadDto>> GetMyRoads(string userId)
        {
            var roads = await _context.Roads
                .Include(x => x.Points)
                .Include(x => x.Ratings)
                .Where(x => x.AuthorId == userId)
                .Select(x => x.ToDto())
                .ToListAsync();
            return roads;
        }

        public async Task<ResponseModel?> RateRoad(CreateRatingDto ratingDto, string userId)
        {
            var road = await _context.Roads.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == ratingDto.ObjectId);
            if (road == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Road with id={ratingDto.ObjectId} not found"
                };
            }
            var isAlreadyRated = await _context.Ratings.FirstOrDefaultAsync(x => x.ObjectId == ratingDto.ObjectId && x.UserId == userId);
            if (isAlreadyRated != null)
            {
                road.RatingSum -= isAlreadyRated.Score;
                road.RatingSum += ratingDto.Rating;
                isAlreadyRated.Score = ratingDto.Rating;
                var oldRating = road.Ratings.FirstOrDefault(x => x.ObjectId == ratingDto.ObjectId);
                oldRating.Score = ratingDto.Rating;
            }
            else
            {
                var rating = new Rating
                {
                    UserId = userId,
                    ObjectId = road.Id,
                    Score = ratingDto.Rating
                };
                await _context.Ratings.AddAsync(rating);
                road.RatingSum += ratingDto.Rating;
                road.Ratings.Add(rating);
            }
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<ResponseModel?> SendRequestToDelete(Guid id)
        {
            var road = await _context.Roads.FirstOrDefaultAsync(x => x.Id == id);
            if (road == null)
            {
                return new ResponseModel
                {
                    Status = "Error: NotFound",
                    Message = $"Road with id={id} not found"
                };
            }
            road.IsToBeDeleted = true;
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<List<RoadDto>> ShowToDeleteRoads()
        {
            return await _context.Roads
                .Include(x => x.Points)
                .Include(x => x.Ratings)
                .Where(x => x.IsToBeDeleted)
                .Select(x => x.ToDto())
                .ToListAsync();
        }
    }
}