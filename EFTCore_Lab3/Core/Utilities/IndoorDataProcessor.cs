using EFTCore_Lab3.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTCore_Lab3.Core.Utilities
{
    public static class IndoorDataProcessor
    {
        public static void FetchIndoorAverageTemperature()
        {
            Console.Write("Ange ett datum (YYYY-MM-DD): ");
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out var specificDate))
            {
                using (var db = new EFContext())
                {
                    var allWeatherData = db.WeatherData.ToList();
                    var inomhus = new Inomhus(allWeatherData);

                    try
                    {
                        var avgTemp = inomhus.GetAverageTemperature(specificDate);
                        Console.WriteLine($"Medeltemperatur för {specificDate:yyyy-MM-dd}: {avgTemp}°C");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt datumformat.");
            }
        }

        public static void SortIndoorDaysByTemperature()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByTemperature = inomhus.GetDaysSortedByTemperature();
                Console.WriteLine("Inomhus dagar sorterade efter medeltemperatur (varmast till kallast):");
                foreach (var (date, avgTemp) in sortedByTemperature)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgTemp}°C");
                }
            }
        }

        public static void SortIndoorDaysByHumidity()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByHumidity = inomhus.GetDaysSortedByHumidity();
                Console.WriteLine("Inomhus dagar sorterade efter medelluftfuktighet (torrast till fuktigast):");
                foreach (var (date, avgHumidity) in sortedByHumidity)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgHumidity}%");
                }
            }
        }

        public static void SortIndoorDaysByMoldRisk()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByMoldRisk = inomhus.GetDaysSortedByMoldRisk();
                Console.WriteLine("Inomhus dagar sorterade efter mögelrisk (minst till störst):");
                foreach (var (date, moldRisk) in sortedByMoldRisk)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {moldRisk:F2}");
                }
            }
        }
    }
}
