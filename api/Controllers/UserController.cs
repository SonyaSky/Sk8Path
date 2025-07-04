using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;
using api.Extensions;
using api.Interfaces;
using api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;

using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        public UserController(IUserService userService, IFileService fileService)
        {
            _userService = userService;
            _fileService = fileService;
        }

        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerOperation("Register a new user")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                if (await _userService.IsUsernameTaken(registerDto.Username))
                {
                    return BadRequest(new ResponseModel
                    {
                        Status = "Error",
                        Message = $"Username {registerDto.Username} is already taken"
                    });
                }

                if (registerDto.FileId != null && !await _fileService.DoesFileExists((Guid)registerDto.FileId))
                {
                    return NotFound(new ResponseModel
                    {
                        Status = "Error",
                        Message = $"File with id={registerDto.FileId} not found"
                    });
                }

                var result = await _userService.RegisterUser(registerDto);
                if (result != null)
                {
                    return Ok(result);
                }
                return StatusCode(500, new ResponseModel
                {
                    Status = "Error",
                    Message = "Couldn't create an applicant"
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("profile")]
        [Authorize]
        [SwaggerOperation("Get current's user profile")]
        public async Task<IActionResult> GetProfile()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var profile = await _userService.GetUserProfile(User.GetId());
            return Ok(profile);
        }

        [ProducesResponseType(typeof(ProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ResponseModel), StatusCodes.Status400BadRequest)]
        [HttpPut("profile")]
        [Authorize]
        [SwaggerOperation("Edit current user's profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileDto profileDto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var profile = await _userService.EditUserProfile(User.GetId(), profileDto);
            if (profile == null)
            {
                return BadRequest(new ResponseModel
                {
                    Status = "Error: Bad Request",
                    Message = $"Username {profileDto.Username} is already taken"
                });
            }
            return Ok(profile);
        }
    }
}