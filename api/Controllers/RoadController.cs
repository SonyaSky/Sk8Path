using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Roads;
using api.Extensions;
using api.Interfaces;

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
        public RoadController(IRoadService roadService)
        {
            _roadService = roadService;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation("Get all roads")]
        public async Task<IActionResult> GetAllRoads()
        {
            var roads = await _roadService.GetAllRoads();
            return Ok(roads);
        }

        [HttpPost]
        [Authorize]
        [SwaggerOperation("Create a new road")]
        public async Task<IActionResult> CreateRoad([FromBody] CreateRoadDto dto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var road = await _roadService.CreateRoad(dto, User.GetId());
            return Ok(road);
        }
    }
}