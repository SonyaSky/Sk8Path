using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Extensions;
using api.Interfaces;
using api.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> AddFile(IFormFile fileDto)
        {
            if (!User.IsAccessToken()) return Unauthorized();
            var fileExtension = Path.GetExtension(fileDto.FileName).TrimStart('.').ToLowerInvariant();

            if (!Enum.GetNames(typeof(Extension)).Any(e => e.ToLowerInvariant() == fileExtension))
            {
                return BadRequest(new ResponseModel
                {
                    Status = "Error",
                    Message = $".{fileExtension} is not a valid extension"
                });
            }

            var file = await _fileService.SaveFile(fileDto, User.GetId());
            return Ok(file);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile([FromRoute] Guid id)
        {
            var result = await _fileService.GetFileAsync(id);
            if (result == null)
            {
                return NotFound(new ResponseModel
                {
                    Status = "Error",
                    Message = $"File with id={id} not found"
                });
            }
            return result;
        }
    }
}