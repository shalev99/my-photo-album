namespace PictureAlbum.API.DTOs
{
    public class FileDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FileContentBase64 { get; set; } // Base64 file content
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string Src { get; set; } // Full src URL for image rendering
    }
}
