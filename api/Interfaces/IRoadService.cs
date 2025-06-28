using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Roads;

namespace api.Interfaces
{
    public interface IRoadService
    {
        Task<List<RoadDto>> GetAllRoads();
        Task<RoadDto> CreateRoad(CreateRoadDto roadDto, string userId);
    }
}