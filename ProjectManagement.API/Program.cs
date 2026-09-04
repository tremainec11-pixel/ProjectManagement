var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine("========================================");
Console.WriteLine("DATABASE CONNECTION CHECK");
Console.WriteLine($"Connection string exists: {!string.IsNullOrEmpty(connectionString)}");

if (!string.IsNullOrEmpty(connectionString))
{
    var connectionBuilder = new Npgsql.NpgsqlConnectionStringBuilder(connectionString);

    Console.WriteLine($"Database Host: {connectionBuilder.Host}");
    Console.WriteLine($"Database Port: {connectionBuilder.Port}");
    Console.WriteLine($"Database Name: {connectionBuilder.Database}");
    Console.WriteLine($"Database User: {connectionBuilder.Username}");
}

Console.WriteLine("========================================");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();