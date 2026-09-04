using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// DATABASE
// =======================================================

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

Console.WriteLine("========================================");
Console.WriteLine("DATABASE CONNECTION CHECK");
Console.WriteLine($"DB_HOST exists: {!string.IsNullOrWhiteSpace(dbHost)}");
Console.WriteLine($"DB_PORT: {dbPort}");
Console.WriteLine($"DB_NAME exists: {!string.IsNullOrWhiteSpace(dbName)}");
Console.WriteLine($"DB_USER exists: {!string.IsNullOrWhiteSpace(dbUser)}");
Console.WriteLine($"DB_PASSWORD exists: {!string.IsNullOrWhiteSpace(dbPassword)}");
Console.WriteLine("========================================");

if (string.IsNullOrWhiteSpace(dbHost) ||
    string.IsNullOrWhiteSpace(dbName) ||
    string.IsNullOrWhiteSpace(dbUser) ||
    string.IsNullOrWhiteSpace(dbPassword))
{
    throw new Exception("Database environment variables are missing.");
}

var connectionString =
    $"Host={dbHost};" +
    $"Port={dbPort};" +
    $"Database={dbName};" +
    $"Username={dbUser};" +
    $"Password={dbPassword};" +
    $"SSL Mode=Require;";

// =======================================================
// SERVICES
// =======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// =======================================================
// APP
// =======================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();