using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Dtos.Roads;
using api.Models;

namespace api.Interfaces
{
    public interface IRoadService
    {
        Task<List<RoadDto>> GetAllRoads();
        Task<RoadDto> CreateRoad(CreateRoadDto roadDto, string userId);
        Task<ResponseModel?> RateRoad(CreateRatingDto ratingDto, string userId);
    }
}