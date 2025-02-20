using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;
using PictureAlbum.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string from User Secrets or environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register DbContext with SQL Server using the secure connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);

// Register IFileService and FileService
builder.Services.AddScoped<IFileService, FileService>();

// Add services for controllers
builder.Services.AddControllers();

// Define CORS policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000", "http://localhost:5162") // Frontend URLs
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials(); // Allow credentials if needed
                      });
});

var app = builder.Build();

// Enable CORS middleware
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Enable Swagger in development mode
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

// Run the application
app.Run();
