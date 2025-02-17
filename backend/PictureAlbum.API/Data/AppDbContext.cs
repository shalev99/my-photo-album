// ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Models;

namespace PictureAlbum.API.Data 
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepting DbContextOptions and passing them to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for FileEntity
        public DbSet<FileEntity> Files { get; set; }
    }
}
