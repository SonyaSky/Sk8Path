using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Dtos.Roads;
using api.Extensions;
using api.Interfaces;
using api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [ApiController]
    [Route("api/road")]
    public class RoadController : ControllerBase
    {
        private readonly IRoadService _roadService;
        private readonly IFileService _fileService;
        public RoadController(IRoadService roadService, IFileService fileService)
        {
            _roadService = roadService;
            _fileService = fileService;
        }

        [ProducesResponseType(typeof(List<RoadDto>), StatusCodes.Status200OK)]
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get all roads")]
        public async Task<IActionResult> GetAllRoads()
        {
            var roads = await _roadService.GetAllRoads();
            return Ok(roads);
        }

        [ProducesResponseType(typeof(RoadDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Authorize]
        [SwaggerOperation("Create a new road")]
        public async Task<IActionResult> CreateRoad([FromBody] CreateRoadDto dto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (dto.FileId != null && !await _fileService.DoesFileExists((Guid)dto.FileId))
            {
                return NotFound(new ResponseModel
                {
                    Status = "Error",
                    Message = $"File with id={dto.FileId} not found"
                });
            }

            var road = await _roadService.CreateRoad(dto, User.GetId());
            return Ok(road);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("rating")]
        [Authorize]
        [SwaggerOperation("Add rating to road")]
        public async Task<IActionResult> AddRatingToSpot([FromBody] CreateRatingDto dto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _roadService.RateRoad(dto, User.GetId());
            if (response == null) return Ok();
            if (response.Status == "Error: NotFound") return NotFound(response);
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(List<RoadDto>), StatusCodes.Status200OK)]
        [HttpGet("my")]
        [Authorize]
        [SwaggerOperation("Get roads created by this user")]
        public async Task<IActionResult> GetMyRoads()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var roads = await _roadService.GetMyRoads(User.GetId());
            return Ok(roads);
        }

        [ProducesResponseType(typeof(List<RatingDto>), StatusCodes.Status200OK)]
        [HttpGet("{id}/rating")]
        [Authorize]
        [SwaggerOperation("Get user's rating of the road")]
        public async Task<IActionResult> GetFavouriteSpots([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var rating = await _roadService.GetRoadRating(id, User.GetId());
            return Ok(rating);
        }

        [HttpPut("{id}/delete")]
        [Authorize]
        [SwaggerOperation("Send request to delete this road")]
        public async Task<IActionResult> SendRequestToDelete([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _roadService.SendRequestToDelete(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Decline request to delete road (for admin)")]
        public async Task<IActionResult> DeclineDeletingSpot([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _roadService.DeclineDeleting(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpDelete("{id}/approve")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Approve request to delete road (for admin)")]
        public async Task<IActionResult> ApproveDeletingSpot([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _roadService.ApproveDeleting(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpGet("deleting")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Get all roads that have request to delete them (for admin)")]
        public async Task<IActionResult> GetAllToDeleteRoads()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _roadService.ShowToDeleteRoads();
            return Ok(response);
        }
    }
}