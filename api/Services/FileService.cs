using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Data;
using api.Interfaces;
using api.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDBContext _context;
        public FileService(IWebHostEnvironment environment, ApplicationDBContext context)
        {
            _environment = environment;
            _context = context;
        }

        public async Task<Guid> SaveFile(IFormFile imageFile, string userId)
        {
            var contentPath = _environment.ContentRootPath;
            var path = Path.Combine(contentPath, "Files");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var ext = Path.GetExtension(imageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fileNameWithPath = Path.Combine(path, fileName);
            using var stream = new FileStream(fileNameWithPath, FileMode.Create);
            await imageFile.CopyToAsync(stream);
            var file = new Models.File
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                FileName = fileName,
            };
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }

        public async Task<FileStreamResult?> GetFileAsync(Guid fileId)
        {
            var fileMeta = await _context.Files.FindAsync(fileId);
            if (fileMeta == null)
                return null;

            var path = Path.Combine(_environment.ContentRootPath, "Files", fileMeta.FileName);
            if (!System.IO.File.Exists(path))
                return null;

            var ext = Path.GetExtension(fileMeta.FileName);
            var contentType = GetContentType(ext);

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(stream, contentType);
        }

        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        public async Task<bool> DoesFileExists(Guid id)
        {
            return await _context.Files.AnyAsync(x => x.Id == id);
        }
    }
}