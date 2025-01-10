using dagnys2.api.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlite(
        builder.Configuration.GetConnectionString("DevConnection")
    )
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var dataContext = services.GetRequiredService<DataContext>();

    await dataContext.Database.MigrateAsync();

    await DataLoader.LoadData(dataContext);
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex.Message}");
    throw;
}

app.MapControllers();

app.Run();
