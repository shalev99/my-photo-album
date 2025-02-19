using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;

namespace PictureAlbum.API.Utils
{
    public static class FileUtils
    {
        // Allowed file extensions (only images and PDFs)
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
        
        // Maximum allowed file size (5MB)
        private static readonly int MaxFileSize = 5 * 1024 * 1024;

        /// <summary>
        /// Validates the uploaded file before processing.
        /// </summary>
        public static void ValidateFile(IFormFile file, string fileName, ApplicationDbContext dbContext)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file uploaded.");

            if (file.Length > MaxFileSize)
                throw new ArgumentException("File size exceeds the allowed limit (5MB).");

            // Validate file extension to prevent unwanted file uploads
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type. Only images and PDFs are allowed.");

            // Ensure file name is safe and formatted correctly
            if (!IsValidFileName(fileName))
                throw new ArgumentException("Invalid file name format.");

            // Prevent duplicate file names in the database
            if (dbContext.Files.Any(f => f.FileName == file.FileName))
                throw new ArgumentException("A file with this name already exists.");

            if (dbContext.Files.Any(f => f.Name == fileName))
                throw new ArgumentException("A picture with this name already exists.");
        }

        /// <summary>
        /// Reads file content as a byte array and Base64 string.
        /// </summary>
        public static async Task<(byte[], string)> ReadFileContentAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] fileContent = memoryStream.ToArray();
            string base64Content = Convert.ToBase64String(fileContent);
            return (fileContent, base64Content);
        }

        /// <summary>
        /// Validates file name format to prevent security vulnerabilities.
        /// </summary>
        public static bool IsValidFileName(string fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && Regex.IsMatch(fileName, @"^[a-zA-Z0-9-_ ]{1,100}$");
        }

        /// <summary>
        /// Safely parses the file date or assigns the current UTC time if invalid.
        /// </summary>
        public static DateTime ParseFileDate(string? fileDate)
        {
            return DateTime.TryParse(fileDate, out var parsedDate) ? parsedDate : DateTime.UtcNow;
        }
    }
}
