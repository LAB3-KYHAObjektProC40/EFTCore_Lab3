using EFTCore_Lab3.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTCore_Lab3.Core.Utilities
{
    public static class BalconyDataProcessor
    {
        public static void CalculateBalconyOpenTime()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var analysis = new TemperatureDifferenceAnalysis(allWeatherData);

                var durations = analysis.CalculateBalconyDoorOpenDuration();

                Console.WriteLine("Balkongdörrens öppettider per dag (sorterat efter längd):");
                foreach (var (Date, Duration) in durations)
                {
                    Console.WriteLine($"{Date:yyyy-MM-dd}: {Duration.TotalHours:F2} timmar");
                }
            }
        }

        public static void SortDaysByTemperatureDifference()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var analysis = new TemperatureDifferenceAnalysis(allWeatherData);

                var differences = analysis.SortByTemperatureDifference();

                Console.WriteLine("Dagar sorterade efter temperaturskillnad (störst till minst):");
                foreach (var (Date, TemperatureDifference) in differences)
                {
                    Console.WriteLine($"{Date:yyyy-MM-dd}: {TemperatureDifference:F2}°C");
                }
            }
        }
    }
}
