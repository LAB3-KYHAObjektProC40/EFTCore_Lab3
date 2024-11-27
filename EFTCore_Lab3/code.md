
# File WetherData.cs

using System.Numerics;

namespace EFTCore_Lab3.Models
{
    public class WeatherData
    {
        public int Id { get; set; }  // Primary key
        public DateTime Datum { get; set; }   // Date and time of the measurement

        public string Plats { get; set; }  // Measurement location (e.g., "Ute", "Inne")
        public double Temp { get; set; }  // Temperature in Celsius

        public int Luftfuktighet { get; set; } // Humidity in percentage

       


    }
}


---

# EFContext.cs


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

---

# Program.cs

using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using System.Collections.Generic;
using EFCore.Models;
using EFTCore_Lab3.Models;



    static void Main(string[] args)
    {
        // Path to the CSV file
        string filePath = @"C:\Users\simon\OneDrive\Dokument\KYH\EXTRA\EFTCore_Lab3\TempFuktData.csv";

        try
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"The file '{filePath}' was not found.");
                return;
            }

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Auto-map the CSV columns to the WeatherData class
                IEnumerable<WeatherData> records = csv.GetRecords<WeatherData>();

                using (var db = new EFContext())
                {
                    // Add the records to the database
                    db.WeatherData.AddRange(records);
                    db.SaveChanges();
                }
            }

            Console.WriteLine("CSV data has been successfully imported into the SQL database.");
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
