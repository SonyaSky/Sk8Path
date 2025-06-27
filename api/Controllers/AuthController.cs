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

namespace api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.Login(loginDto);

            if (result == null) {
                return BadRequest(new ResponseModel {
                    Status = "Error",
                    Message = "Login failed"
                });
            }
            
            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var tokens = await _authService.RefreshToken(refreshDto);
            if (tokens == null) {
                return Unauthorized();
            }
            return Ok(tokens);
        }
        
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var message = await _authService.Logout(User.GetId());
            if (message.Status == "Error") {
                return Unauthorized();
            }
            return Ok();
        }
    }
}