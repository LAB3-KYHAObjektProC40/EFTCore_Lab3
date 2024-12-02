using System;
using System.Collections.Generic;
using System.Linq;
using EFTCore_Lab3.DataAccess;
using EFTCore_Lab3.Core.Models;

namespace EFTCore_Lab3.Core.Utilities
{
    public static class CsvImporter
    {
        public static void ImportAndSaveCsvData()
        {
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "weatherdata.csv");

            Console.WriteLine($"Looking for file at: {csvFilePath}");

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine("CSV file not found. Please ensure it exists in the 'Resources' folder.");
                return;
            }

            try
            {
                var weatherData = CsvHelperService.ReadCsvFile(csvFilePath);
                DatabaseHelper.SaveToDatabase(weatherData);
                Console.WriteLine($"{weatherData.Count} records successfully saved to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        /// Loads only indoor weather data from the database.

        public static List<WeatherData> LoadIndoorData()
        {
            using (var db = new EFContext())
            {
                return db.WeatherData
                    .Where(data => data.Plats.ToLower() == "inne")
                    .ToList();
            }
        }



        /// Loads only outdoor weather data from the database.

        public static List<WeatherData> LoadOutdoorData()
        {
            using (var db = new EFContext())
            {
                return db.WeatherData
                    .Where(data => data.Plats.ToLower() == "ute")
                    .ToList();
            }
        }
    }
}
