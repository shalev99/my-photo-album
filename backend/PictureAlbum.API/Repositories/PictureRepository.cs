using Microsoft.EntityFrameworkCore;
public class PictureRepository
{
    private readonly AppDbContext _context;
    public PictureRepository(AppDbContext context) { _context = context; }

    public async Task<List<Picture>> GetAllAsync() => await _context.Pictures.ToListAsync();

    public async Task<Picture?> GetByNameAsync(string name) => await _context.Pictures.FirstOrDefaultAsync(p => p.Name == name);

    public async Task<int> AddAsync(Picture picture)
    {
        _context.Pictures.Add(picture);
        await _context.SaveChangesAsync();
        return picture.Id;
    }
}
