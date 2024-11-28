using Microsoft.EntityFrameworkCore;
using WeatherDataApp.Models;

namespace WeatherDataApp.Data
{
    public class EFContext : DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WeatherDataDB;Trusted_Connection=True;");
        }

        public DbSet<WeatherData> WeatherData { get; set; }
    }
}
