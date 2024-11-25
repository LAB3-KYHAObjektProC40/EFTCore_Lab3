
using EFTCore_Lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Models;

public class EFContext : DbContext

{
 private const string connectionString =
"Server=(localdb)\\MSSQLLocalDB;Database=EFCore;Trusted_Connection=True;";
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer(connectionString);
}
    public DbSet<WeatherData> WeatherData { get; set; }
}