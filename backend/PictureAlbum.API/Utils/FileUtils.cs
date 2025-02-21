using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;

namespace PictureAlbum.API.Utils
{
    /// <summary>
    /// Utility class for handling file operations.
    /// </summary>
    public static class FileUtils
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

        /// <summary>
        /// Validates an uploaded file.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="ArgumentException">Thrown when validation fails.</exception>
        public static async Task ValidateFileAsync(IFormFile file, string fileName, ApplicationDbContext dbContext)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File size exceeds the allowed limit (5MB).");

            string fileExtension = Path.GetExtension(file.FileName)?.ToLower();
            if (string.IsNullOrEmpty(fileExtension) || !AllowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type. Only images and PDFs are allowed.");

            string safeFileName = Path.GetFileName(file.FileName); // Prevents path traversal attacks
            if (!IsValidFileName(fileName))
                throw new ArgumentException("Invalid file name format.");

            // Optimized query to check for duplicate files in a single call
            var existingFile = await dbContext.Files
                .Where(f => f.FileName == safeFileName || f.Name == fileName)
                .Select(f => new { f.FileName, f.Name })
                .FirstOrDefaultAsync();

            if (existingFile != null)
            {
                if (existingFile.FileName == safeFileName)
                    throw new InvalidOperationException("A file with this name already exists.");

                if (existingFile.Name == fileName)
                    throw new InvalidOperationException("A picture with this name already exists.");
            }
        }

        /// <summary>
        /// Reads the file content and returns both the byte array and Base64 string.
        /// </summary>
        /// <param name="file">The uploaded file.</param>
        /// <returns>A tuple containing the file's byte array and its Base64 representation.</returns>
        public static async Task<(byte[] FileBytes, string Base64)> ReadFileContentAsync(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileContent = memoryStream.ToArray();
            return (fileContent, Convert.ToBase64String(fileContent));
        }

        /// <summary>
        /// Validates a file name to ensure it follows a safe format.
        /// </summary>
        /// <param name="fileName">The file name to validate.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public static bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) &&
                   Regex.IsMatch(fileName, @"^[a-zA-Z0-9-_ ]{1,100}$");
        }

        /// <summary>
        /// Parses a file date string, returning the current UTC time if invalid.
        /// </summary>
        /// <param name="fileDate">The file date as a string.</param>
        /// <returns>A valid DateTime object.</returns>
        public static DateTime ParseFileDate(string? fileDate)
        {
            return DateTime.TryParse(fileDate, out var parsedDate) ? parsedDate : DateTime.UtcNow;
        }
    }
}
