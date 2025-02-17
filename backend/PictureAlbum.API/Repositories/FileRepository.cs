using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Models;
using PictureAlbum.API.Data;

namespace PictureAlbum.API.Repositories
{
    public class FileRepository
    {
        private readonly ApplicationDbContext _context;

        public FileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all files
        public async Task<List<FileEntity>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }

        // Get a file by name
        public async Task<FileEntity?> GetByNameAsync(string name)
        {
            return await _context.Files
                .FirstOrDefaultAsync(file => file.Name == name);
        }

        // Add a new file
        public async Task<int> AddAsync(FileEntity fileEntity)
        {
            _context.Files.Add(fileEntity);
            await _context.SaveChangesAsync();
            return fileEntity.Id; // return the ID of the newly added file
        }
    }
}
