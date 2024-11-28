using System;
using System.Collections.Generic;
using System.Linq;
using EFTCore_Lab3.Models;


namespace EFTCore_Lab3.Utilities
{
    public class Utomhus
    {
        private readonly List<WeatherData> _weatherData;

        public Utomhus(List<WeatherData> weatherData)
        {
            // Filter only "Ute" data
            _weatherData = weatherData.Where(data => data.Plats.Equals("Ute", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // 1. Average temperature for a specific date
        public float GetAverageTemperature(DateTime date)
        {
            var dailyData = _weatherData.Where(data => data.Datum.Date == date.Date);

            return dailyData.Any()
                ? dailyData.Average(data => data.Temp)
                : throw new ArgumentException("No data available for the selected date.");
        }

        // 2. Sort days by average temperature (warmest to coldest)
        public List<(DateTime Date, float AverageTemperature)> GetDaysSortedByTemperature()
        {
            return _weatherData
                .GroupBy(data => data.Datum.Date)
                .Select(g => new { Date = g.Key, AverageTemperature = g.Average(data => data.Temp) })
                .OrderByDescending(result => result.AverageTemperature)
                .Select(result => (result.Date, result.AverageTemperature))
                .ToList();
        }

        // 3. Sort days by average humidity (driest to most humid)
        public List<(DateTime Date, float AverageHumidity)> GetDaysSortedByHumidity()
        {
            return _weatherData
                .GroupBy(data => data.Datum.Date)
                .Select(g => new { Date = g.Key, AverageHumidity = (float)g.Average(data => data.Luftfuktighet) })
                .OrderBy(result => result.AverageHumidity)
                .Select(result => (result.Date, result.AverageHumidity))
                .ToList();
        }



        // 4. Sort days by risk of mold (least to greatest)
        public List<(DateTime Date, float MoldRisk)> GetDaysSortedByMoldRisk()
        {
            return _weatherData
                .GroupBy(data => data.Datum.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    MoldRisk = g.Average(data => CalculateMoldRisk(data.Temp, data.Luftfuktighet))
                })
                .OrderBy(result => result.MoldRisk)
                .Select(result => (result.Date, result.MoldRisk))
                .ToList();
        }

        // 5. Date for meteorological autumn
        public DateTime? GetDateForAutumn()
        {
            var autumnDays = _weatherData
                .GroupBy(data => data.Datum.Date)
                .Where(g => g.Average(data => data.Temp) <= 10)
                .OrderBy(g => g.Key)
                .ToList();

            // Check for 5 consecutive days below 10°C
            for (int i = 0; i < autumnDays.Count - 4; i++)
            {
                if (autumnDays.Skip(i).Take(5).All(g => g.Average(data => data.Temp) <= 10))
                {
                    return autumnDays[i].Key;
                }
            }

            return null; // No autumn detected
        }

        // 6. Date for meteorological winter
        public DateTime? GetDateForWinter()
        {
            var winterDays = _weatherData
                .GroupBy(data => data.Datum.Date)
                .Where(g => g.Average(data => data.Temp) <= 0)
                .OrderBy(g => g.Key)
                .ToList();

            // Check for 5 consecutive days below 0°C
            for (int i = 0; i < winterDays.Count - 4; i++)
            {
                if (winterDays.Skip(i).Take(5).All(g => g.Average(data => data.Temp) <= 0))
                {
                    return winterDays[i].Key;
                }
            }

            return null; // No winter detected
        }

        // Helper method to calculate mold risk
        private static float CalculateMoldRisk(float temperature, int humidity)
        {
            // Mold risk formula: temperature * (humidity / 100), only if humidity > 70 and temperature >= 0
            return humidity > 70 && temperature >= 0 ? temperature * (humidity / 100f) : 0;
        }
    }
}
