using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using PictureAlbum.API.Data;
using PictureAlbum.API.Models;
using System.IO;
Console.WriteLine("Reached this part of the code");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services for controllers (you'll need a controller for your API)
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add a route to upload files
app.MapPost("/api/files", async (IFormFile file, string description, ApplicationDbContext dbContext) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // Ensure directory exists to save files
    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    if (!Directory.Exists(uploadDirectory))
    {
        Directory.CreateDirectory(uploadDirectory);
    }

    var filePath = Path.Combine(uploadDirectory, file.FileName);

    // Save the file to the server
    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    // Save file information to the database
    var fileEntity = new FileEntity
    {
        Name = file.FileName,  // Name of the picture (e.g., "sunset.jpg")
        FileName = file.FileName,  // The actual file name (e.g., "sunset.jpg")
        FileType = file.ContentType,  // The type of the file (e.g., "image/jpeg")
        FileSize = file.Length,  // The size of the file in bytes
        Description = description,  // Optional description provided by the user
        UploadDate = DateTime.Now  // The date and time when the picture was uploaded
    };


    dbContext.Files.Add(fileEntity);
    await dbContext.SaveChangesAsync();

    return Results.Ok(new { fileEntity.Id, fileEntity.Name });
});

// Add a route to get all files
app.MapGet("/api/files", async (ApplicationDbContext dbContext) =>
{
    var files = await dbContext.Files.ToListAsync();
    return Results.Ok(files);
});

app.Run();
