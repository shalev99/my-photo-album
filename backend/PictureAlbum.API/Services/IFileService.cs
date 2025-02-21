using PictureAlbum.API.Models;

namespace PictureAlbum.API.Services
{
    /// <summary>
    /// Defines the contract for file-related operations.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Uploads a file after performing necessary validations.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <param name="fileName">The user-defined file name.</param>
        /// <param name="fileDate">The optional file date.</param>
        /// <param name="fileDescription">The optional file description.</param>
        /// <returns>The uploaded file entity.</returns>
        Task<FileEntity> UploadFileAsync(IFormFile file, string fileName, string? fileDate, string? fileDescription);

        /// <summary>
        /// Retrieves a list of all stored files.
        /// </summary>
        /// <returns>A list of <see cref="FileEntity"/> objects.</returns>
        Task<List<FileEntity>> GetFilesAsync();

        /// <summary>
        /// Retrieves a paginated list of stored files.
        /// </summary>
        /// <param name="page">The page number (1-based).</param>
        /// <param name="pageSize">The number of files per page.</param>
        /// <returns>A paginated list of <see cref="FileEntity"/> objects.</returns>
        Task<List<FileEntity>> GetFilesAsync(int page, int pageSize);
    }
}
