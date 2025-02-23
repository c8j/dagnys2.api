using dagnys2.api.Data;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(9, 2, 0));

builder.Services.AddDbContext<DataContext>(
/* options => options.UseSqlite(
    builder.Configuration.GetConnectionString("DevConnection")
) */
    options => options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion)
);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

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
