public class PictureService
{
    private readonly PictureRepository _repository;
    public PictureService(PictureRepository repository) { _repository = repository; }

    public async Task<List<Picture>> GetPicturesAsync() => await _repository.GetAllAsync();

    public async Task<(bool Success, string Message, int Id)> AddPictureAsync(Picture picture)
    {
        if (await _repository.GetByNameAsync(picture.Name) != null)
            return (false, "Picture name already exists", 0);

        int id = await _repository.AddAsync(picture);
        return (true, "Success", id);
    }
}
