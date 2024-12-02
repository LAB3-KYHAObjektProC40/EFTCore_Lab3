using EFTCore_Lab3.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTCore_Lab3.Core.Utilities
{
    public static class OutdoorDataProcessor
    {

        public static void FetchOutdoorAverageTemperature()
        {
            using (var db = new EFContext())
            {
                var outdoorData = db.WeatherData
                    .AsEnumerable() // Fetch data from the database first
                    .Where(data => data.Plats.Equals("Ute", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Console.Write("Ange ett datum (YYYY-MM-DD): ");
                string? input = Console.ReadLine();

                if (DateTime.TryParse(input, out var specificDate))
                {
                    try
                    {
                        var avgTemp = outdoorData
                            .Where(data => data.Datum.Date == specificDate.Date)
                            .Average(data => data.Temp);

                        Console.WriteLine($"Medeltemperatur för {specificDate:yyyy-MM-dd}: {avgTemp:F2}°C");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("No data available for the specified date.");
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt datumformat.");
                }
            }
        }



        public static void SortOutdoorDaysByTemperature()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByTemperature = utomhus.GetDaysSortedByTemperature();
                Console.WriteLine("Dagar sorterade efter medeltemperatur (varmast till kallast):");
                foreach (var (date, avgTemp) in sortedByTemperature)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgTemp}°C");
                }
            }
        }

        public static void SortOutdoorDaysByHumidity()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByHumidity = utomhus.GetDaysSortedByHumidity();
                Console.WriteLine("Dagar sorterade efter medelluftfuktighet (torrast till fuktigast):");
                foreach (var (date, avgHumidity) in sortedByHumidity)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgHumidity}%");
                }
            }
        }

        public static void SortOutdoorDaysByMoldRisk()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByMoldRisk = utomhus.GetDaysSortedByMoldRisk();
                Console.WriteLine("Dagar sorterade efter mögelrisk (minst till störst):");
                foreach (var (date, moldRisk) in sortedByMoldRisk)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {moldRisk:F2}");
                }
            }
        }

        public static void GetMeteorologicalAutumnDate()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var autumnDate = utomhus.GetDateForAutumn();
                if (autumnDate.HasValue)
                    Console.WriteLine($"Datum för meteorologisk Höst: {autumnDate.Value:yyyy-MM-dd}");
                else
                    Console.WriteLine("Ingen meteorologisk Höst hittades.");
            }
        }

        public static void GetMeteorologicalWinterDate()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var winterDate = utomhus.GetDateForWinter();
                if (winterDate.HasValue)
                    Console.WriteLine($"Datum för meteorologisk Vinter: {winterDate.Value:yyyy-MM-dd}");
                else
                    Console.WriteLine("Ingen meteorologisk Vinter hittades.");
            }
        }


    }
}
