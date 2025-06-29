using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Dtos.Spots;
using api.Models;

namespace api.Interfaces
{
    public interface ISpotService
    {
        Task<SpotDto> CreateSpot(CreateSpotDto spotDto, string userId);
        Task<List<SpotDto>> GetAllSpots();
        Task<ResponseModel?> RateSpot(CreateRatingDto ratingDto, string userId);
        Task<List<SpotDto>> GetMySpots(string userId);
        Task<List<SpotDto>> ShowToDeleteSpots();
        Task<ResponseModel?> SendRequestToDelete(Guid id);
        Task<ResponseModel?> ApproveDeleting(Guid id);
        Task<ResponseModel?> DeclineDeleting(Guid id);
        Task<List<SpotDto>> ShowFavouriteSpots(string userId);
        Task<ResponseModel?> AddToFavourites(Guid id, string userId);
        Task<ResponseModel?> RemoveFromFavourites(Guid id, string userId);
    }
}