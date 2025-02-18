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
app.MapPost("/api/files", async (HttpContext httpContext, ApplicationDbContext dbContext) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    var file = form.Files["pictureFile"];
    var name = form["pictureName"];
    var date = form["pictureDate"];
    var description = form["pictureDescription"];

    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded.");
    }

    // Read file content into a byte array
    byte[] fileContent;
    using (var memoryStream = new MemoryStream())
    {
        await file.CopyToAsync(memoryStream);
        fileContent = memoryStream.ToArray();
    }

    // Ensure directory exists to save files
    var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
    if (!Directory.Exists(uploadDirectory))
    {
        Directory.CreateDirectory(uploadDirectory);
    }

    // Generate a unique file name
    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var filePath = Path.Combine(uploadDirectory, uniqueFileName);

    try
    {
        // Save file to server
        await File.WriteAllBytesAsync(filePath, fileContent);

        // Save file info to DB
        var fileEntity = new FileEntity
        {
            Name = name,
            FileName = uniqueFileName,
            FileType = file.ContentType,
            FileSize = file.Length,
            FileContent = fileContent, // ✅ Storing the file content in the database
            Description = description,
            UploadDate = DateTime.TryParse(date, out var parsedDate) ? parsedDate : DateTime.UtcNow // ✅ Handling date parsing
        };

        dbContext.Files.Add(fileEntity);
        await dbContext.SaveChangesAsync();

        return Results.Ok(new { fileEntity.Id, fileEntity.Name, fileEntity.FileName });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
})
.DisableAntiforgery();



// Add a route to get all files
app.MapGet("/api/files", async (ApplicationDbContext dbContext) =>
{
    var files = await dbContext.Files.ToListAsync();
    return Results.Ok(files);
});

// Run the application
app.Run();
