using PictureAlbum.API.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using PictureAlbum.API.Models;

namespace PictureAlbum.API.Services
{
    public interface IFileService
    {
        Task<FileEntity> UploadFileAsync(IFormFile file, string fileName, string fileDate, string fileDescription);
        Task<List<FileEntity>> GetFilesAsync();
    }

}
