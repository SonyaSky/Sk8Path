using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

namespace api.Interfaces
{
    public interface IFileService
    {
        Task<Guid> SaveFile(IFormFile file, string userId);
        Task<FileStreamResult?> GetFileAsync(Guid fileId);
        Task<bool> DoesFileExists(Guid id);
    }
}