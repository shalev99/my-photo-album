using PictureAlbum.API.Models;
using PictureAlbum.API.Repositories;

public class FileService
{
    private readonly FileRepository _repository;

    // Constructor
    public FileService(FileRepository repository) 
    { 
        _repository = repository; 
    }

    // Get a list of all files
    public async Task<List<FileEntity>> GetFilesAsync() 
    {
        return await _repository.GetAllAsync();
    }

    // Add a file to the database
    public async Task<(bool Success, string Message, int Id)> AddFileAsync(FileEntity fileEntity)
    {
        // Check if a file with the same name already exists
        if (await _repository.GetByNameAsync(fileEntity.Name) != null)
        {
            return (false, "File name already exists", 0);
        }

        // Add file to the repository and return success message with the new file ID
        int id = await _repository.AddAsync(fileEntity);
        return (true, "Success", id);
    }
}
