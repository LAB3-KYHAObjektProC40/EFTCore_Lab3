using Microsoft.EntityFrameworkCore;
using EFTCore_Lab3.Models;

namespace EFTCore_Lab3.Data
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
