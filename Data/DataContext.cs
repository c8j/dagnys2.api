using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
}
