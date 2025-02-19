namespace PictureAlbum.API.Models
{
    public class FileEntity
    {
        public int Id { get; set; }  // Unique identifier for the picture
        public string Name { get; set; }  // The name of the picture (e.g., "Beach Sunset")
        public string FileName { get; set; }  // The actual file name of the uploaded picture (e.g., "sunset.jpg")
        public string FileType { get; set; }  // The type of the file (e.g., "jpg", "png")
        public long FileSize { get; set; }  // The size of the file in bytes
        public string? Description { get; set; }  // Optional description for the picture (up to 250 characters)
        public byte[] FileContent { get; set; }  // The binary content of the file (actual image data)
        public DateTime UploadDate { get; set; }  // The date and time when the picture was uploaded

    }
}
