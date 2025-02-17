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

// Add services for controllers
builder.Services.AddControllers();

// Define CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Configure CORS to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Allow both frontend origins (React and potential other origin)
                          policy.WithOrigins("http://localhost:3000", "http://localhost:5162") // Frontend URLs
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();  // Allow credentials if needed
                      });
});

var app = builder.Build();

// Enable CORS middleware
app.UseCors(MyAllowSpecificOrigins); 

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

    // Generate a unique file name to avoid conflicts
    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadDirectory, uniqueFileName);

    try
    {
        // Save the file to the server
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Save file information to the database
        var fileEntity = new FileEntity
        {
            Name = file.FileName,  // Original file name
            FileName = uniqueFileName,  // Unique file name saved on server
            FileType = file.ContentType,  // The type of the file (e.g., "image/jpeg")
            FileSize = file.Length,  // The size of the file in bytes
            Description = description,  // Optional description provided by the user
            UploadDate = DateTime.Now  // The date and time when the picture was uploaded
        };

        // Add the file information to the database
        dbContext.Files.Add(fileEntity);
        await dbContext.SaveChangesAsync();

        // Return a response with file details (ID and name)
        return Results.Ok(new { fileEntity.Id, fileEntity.Name, fileEntity.FileName });
    }
    catch (Exception ex)
    {
        // Return an internal server error if something goes wrong
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
});


// Add a route to get all files
app.MapGet("/api/files", async (ApplicationDbContext dbContext) =>
{
    var files = await dbContext.Files.ToListAsync();
    return Results.Ok(files);
});

// Run the application
app.Run();
