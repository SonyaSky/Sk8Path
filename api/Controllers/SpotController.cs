using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Dtos.Spots;
using api.Extensions;
using api.Interfaces;
using api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [ApiController]
    [Route("api/spot")]
    public class SpotController : ControllerBase
    {
        private readonly ISpotService _spotService;
        public SpotController(ISpotService spotService)
        {
            _spotService = spotService;
        }

        [ProducesResponseType(typeof(List<SpotDto>), StatusCodes.Status200OK)]
        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get all spots")]
        public async Task<IActionResult> GetAllSpots()
        {
            var spots = await _spotService.GetAllSpots();
            return Ok(spots);
        }

        [ProducesResponseType(typeof(SpotDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        [Authorize]
        [SwaggerOperation("Create a new spot")]
        public async Task<IActionResult> CreateSpot([FromBody] CreateSpotDto spotDto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var spot = await _spotService.CreateSpot(spotDto, User.GetId());
            return Ok(spot);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("rating")]
        [Authorize]
        [SwaggerOperation("Add rating to spot")]
        public async Task<IActionResult> AddRatingToSpot([FromBody] CreateRatingDto dto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _spotService.RateSpot(dto, User.GetId());
            if (response == null) return Ok();
            if (response.Status == "Error: NotFound") return NotFound(response);
            return BadRequest(response);
        }

        [ProducesResponseType(typeof(List<SpotDto>), StatusCodes.Status200OK)]
        [HttpGet("my")]
        [Authorize]
        [SwaggerOperation("Get spots created by this user")]
        public async Task<IActionResult> GetMySpots()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var spots = await _spotService.GetMySpots(User.GetId());
            return Ok(spots);
        }

        [HttpPut("{id}/delete")]
        [Authorize]
        [SwaggerOperation("Send request to delete this spot")]
        public async Task<IActionResult> SendRequestToDelete([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _spotService.SendRequestToDelete(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Decline request to delete spot (for admin)")]
        public async Task<IActionResult> DeclineDeletingSpot([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _spotService.DeclineDeleting(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpDelete("{id}/approve")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Approve request to delete spot (for admin)")]
        public async Task<IActionResult> ApproveDeletingSpot([FromRoute] Guid id)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _spotService.ApproveDeleting(id);
            if (response == null) return Ok();
            return NotFound(response);
        }

        [HttpGet("deleting")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation("Get all spots that have request to delete them (for admin)")]
        public async Task<IActionResult> GetAllToDeleteSpots()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var response = await _spotService.ShowToDeleteSpots();
            return Ok(response);
        }
    }
}