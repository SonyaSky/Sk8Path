using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Dtos.Spots;
using api.Extensions;
using api.Interfaces;

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

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get all spots")]
        public async Task<IActionResult> GetAllSpots()
        {
            var spots = await _spotService.GetAllSpots();
            return Ok(spots);
        }

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
    }
}