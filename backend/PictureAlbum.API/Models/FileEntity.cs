using System.ComponentModel.DataAnnotations.Schema;

namespace PictureAlbum.API.Models
{
    public class FileEntity
    {
        public int Id { get; set; }  // Unique identifier for the file
        public string Name { get; set; }  // The name of the file (e.g., "Beach Sunset")
        public string FileName { get; set; }  // The actual file name of the uploaded file (e.g., "sunset.jpg")
        public string FileType { get; set; }  // The type of the file (e.g., "jpg", "png")
        public long FileSize { get; set; }  // The size of the file in bytes
        public string? Description { get; set; }  // Optional description for the file (up to 250 characters)
        public byte[] FileContent { get; set; }  // The binary content of the file (actual image data)
        public DateTime UploadDate { get; set; }  // The date and time when the file was uploaded

        // This property should NOT be mapped to the database
        [NotMapped]
        public string FileContentBase64 { get; set; } // Base64 file content


    }
}
