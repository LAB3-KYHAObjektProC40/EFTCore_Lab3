using EFTCore_Lab3.Data;
using EFTCore_Lab3.Models;
using Microsoft.EntityFrameworkCore; // Required for EF Core classes
using System.Collections.Generic;

namespace EFTCore_Lab3.Utilities
{
    public static class DatabaseHelper
    {
        public static void SaveToDatabase(List<WeatherData> weatherDataList)
        {
            using (var db = new EFContext())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // Optional: Can improve performance by not tracking entities during bulk insert

                db.WeatherData.AddRange(weatherDataList); // Add all records from the CSV
                db.SaveChanges(); // Save to database
            }
        }
    }
}
