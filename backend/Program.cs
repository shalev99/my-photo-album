using Microsoft.EntityFrameworkCore;
using PictureAlbum.API.Data;

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

// Add a route to get all files (this can be handled by the FileController)
app.MapControllers();

// Run the application
app.Run();
