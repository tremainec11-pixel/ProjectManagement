using Microsoft.EntityFrameworkCore;
using ProjectManagement.API.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine("========================================");
Console.WriteLine("DATABASE CONNECTION CHECK");
Console.WriteLine($"Connection string exists: {!string.IsNullOrEmpty(connectionString)}");

if (!string.IsNullOrEmpty(connectionString))
{
    var connectionBuilder = new NpgsqlConnectionStringBuilder(connectionString);

    Console.WriteLine($"Database Host: {connectionBuilder.Host}");
    Console.WriteLine($"Database Port: {connectionBuilder.Port}");
    Console.WriteLine($"Database Name: {connectionBuilder.Database}");
    Console.WriteLine($"Database User: {connectionBuilder.Username}");
    Console.WriteLine($"Password provided: {!string.IsNullOrEmpty(connectionBuilder.Password)}");
}

Console.WriteLine("========================================");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();