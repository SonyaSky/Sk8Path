using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Spots;

namespace api.Interfaces
{
    public interface ISpotService
    {
        Task<SpotDto> CreateSpot(CreateSpotDto spotDto, string userId);
        Task<List<SpotDto>> GetAllSpots();
    }
}