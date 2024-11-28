using WeatherDataApp.Data;
using WeatherDataApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace WeatherDataApp.Utilities
{
    public static class DatabaseHelper
    {
        public static void SaveToDatabase(List<WeatherData> weatherDataList)
        {
            using (var db = new EFContext())
            {
                // Get all existing records from the database
                var existingData = db.WeatherData
                    .Select(w => new { w.Datum, w.Temp })
                    .ToHashSet(); // Efficient lookup for duplicates

                // Filter out duplicates from the incoming list
                var uniqueData = weatherDataList
                    .Where(newData => !existingData.Contains(new { newData.Datum, newData.Temp }))
                    .ToList();

                // Add only the unique data to the database
                db.WeatherData.AddRange(uniqueData);
                db.SaveChanges();
            }
        }
    }
}

