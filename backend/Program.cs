using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi(); // Enable OpenAPI/Swagger UI

// Register DbContext to use SQL Server with the connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register IFileService and FileService
builder.Services.AddScoped<IFileService, FileService>();

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
    app.MapOpenApi();  // Map OpenAPI endpoints for Swagger
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
