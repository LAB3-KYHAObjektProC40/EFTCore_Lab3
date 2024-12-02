using EFTCore_Lab3.Core.Models;
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

        public static void SortIndoorDaysByMoldRisk(List<WeatherData> indoorData)
        {
            var groupedIndoor = indoorData.GroupBy(data => data.Datum.Date);

            var moldRiskList = new List<(DateTime Date, double MoldRisk)>();

            foreach (var group in groupedIndoor)
            {
                var date = group.Key;
                var averageTemp = group.Average(data => data.Temp);
                var averageHumidity = group.Average(data => data.Luftfuktighet);

                // Simple mold risk calculation
                double moldRisk = CalculateMoldRisk(averageTemp, averageHumidity);

                moldRiskList.Add((date, moldRisk));
            }

            var sortedMoldRiskList = moldRiskList.OrderBy(r => r.MoldRisk).ToList();

            Console.WriteLine("Date\t\tMold Risk");
            foreach (var item in sortedMoldRiskList)
            {
                Console.WriteLine($"{item.Date.ToShortDateString()}\t{item.MoldRisk:F2}");
            }
        }

        private static double CalculateMoldRisk(double temperature, double humidity)
        {
            // Simple mold risk formula: (humidity - 70) * (temperature / 25)
            // Adjust the formula as needed for your specific requirements
            return (humidity - 70) * (temperature / 25);
        }
    }
}
