using EFTCore_Lab3.Core.Models;
using EFTCore_Lab3.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFTCore_Lab3.DataAccess
{
    public static class DatabaseHelper
    {
        public static void SaveToDatabase(List<WeatherData> weatherDataList)
        {
            using (var db = new EFContext())
            {
                // Step 1: Fetch existing data from the database
                var existingData = db.WeatherData
                    .Select(w => new
                    {
                        w.Datum,
                        Temp = Math.Round(w.Temp, 2), // Normalize temperature for consistent comparison
                        Plats = w.Plats.Trim().ToLowerInvariant() // Normalize location string
                    })
                    .ToHashSet();

                // Step 2: Filter out duplicates from the incoming data
                var uniqueData = weatherDataList
                    .Where(newData => !existingData.Contains(new
                    {
                        newData.Datum,
                        Temp = Math.Round(newData.Temp, 2), // Normalize temperature
                        Plats = newData.Plats.Trim().ToLowerInvariant() // Normalize location
                    }))
                    .ToList();

                // Step 3: Save unique records and print a single message
                if (uniqueData.Any())
                {
                    db.WeatherData.AddRange(uniqueData);
                    db.SaveChanges();
                    Console.WriteLine($"{uniqueData.Count} unique records successfully saved to the database.");
                }
                else
                {
                    Console.WriteLine("No new records to save. All data already exists in the database.");
                }
            }
        }
    }
}
