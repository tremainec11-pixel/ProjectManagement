using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Exceptions;
using ProjectManagement.Application.Interfaces;
using ProjectManagement.Application.Services;
using ProjectManagement.Infrastructure.Data;
using ProjectManagement.Infrastructure.Services;

Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "true");

var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers
// =========================

builder.Services.AddControllers();

// =========================
// CORS
// =========================

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? new[]
    {
        "http://localhost:4200"
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =========================
// Database
// =========================

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

Console.WriteLine("=================================");
Console.WriteLine(
    $"DATABASE CONNECTION CONFIGURED: {!string.IsNullOrWhiteSpace(connectionString)}");
Console.WriteLine("=================================");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
    
// =========================
// Services
// =========================

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectMemberService, ProjectMemberService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

// =========================
// Global Exception Handling
// =========================

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// =========================
// Swagger
// =========================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =========================
// Global Exception Handling Middleware
// =========================

app.UseExceptionHandler();

// =========================
// CORS
// =========================

app.UseCors("AllowAngular");

// =========================
// Swagger
// =========================

app.UseSwagger();
app.UseSwaggerUI();

// =========================
// HTTP
// =========================

// HTTPS redirection disabled for Render
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

