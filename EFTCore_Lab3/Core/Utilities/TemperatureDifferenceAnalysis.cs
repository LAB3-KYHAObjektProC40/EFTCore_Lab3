using System;
using System.Collections.Generic;
using System.Linq;
using EFTCore_Lab3.Core.Models;

namespace EFTCore_Lab3.Core.Utilities
{
    public class TemperatureDifferenceAnalysis
    {
        private readonly List<WeatherData> _indoorData;
        private readonly List<WeatherData> _outdoorData;

        // Constructor to filter data for indoor and outdoor
        public TemperatureDifferenceAnalysis(List<WeatherData> weatherData)
        {
            _indoorData = weatherData.Where(data => data.Plats.Equals("Inne", StringComparison.OrdinalIgnoreCase)).ToList();
            _outdoorData = weatherData.Where(data => data.Plats.Equals("Ute", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Calculate temperature differences grouped by date
        public List<(DateTime Date, float TemperatureDifference)> SortByTemperatureDifference()
        {
            var differences = new List<(DateTime Date, float TemperatureDifference)>();

            // Group indoor and outdoor data by date
            var groupedIndoor = _indoorData.GroupBy(data => data.Datum.Date);
            var groupedOutdoor = _outdoorData.GroupBy(data => data.Datum.Date);

            foreach (var indoorGroup in groupedIndoor)
            {
                var date = indoorGroup.Key; // Current date
                var outdoorGroup = groupedOutdoor.FirstOrDefault(g => g.Key == date);

                if (outdoorGroup == null) continue; // No matching outdoor data for this date

                // Calculate average temperatures for indoor and outdoor
                float indoorAverage = indoorGroup.Average(data => data.Temp);
                float outdoorAverage = outdoorGroup.Average(data => data.Temp);

                // Calculate absolute temperature difference
                float difference = Math.Abs(indoorAverage - outdoorAverage);
                differences.Add((date, difference));
            }

            // Sort by difference descending
            return differences.OrderByDescending(d => d.TemperatureDifference).ToList();
        }

        // Calculate the duration the balcony door is open per day
        public List<(DateTime Date, TimeSpan Duration)> CalculateBalconyDoorOpenDuration()
        {
            var durations = new List<(DateTime Date, TimeSpan Duration)>();

            // Group indoor and outdoor data by date
            var groupedIndoor = _indoorData.GroupBy(data => data.Datum.Date);
            var groupedOutdoor = _outdoorData.GroupBy(data => data.Datum.Date);

            foreach (var indoorGroup in groupedIndoor)
            {
                var date = indoorGroup.Key; // Current date
                var outdoorGroup = groupedOutdoor.FirstOrDefault(g => g.Key == date);

                if (outdoorGroup == null) continue; // No matching outdoor data for this date

                // Track the duration when the door is open
                TimeSpan totalDuration = TimeSpan.Zero;
                DateTime? openStartTime = null;

                foreach (var indoorData in indoorGroup)
                {
                    var correspondingOutdoorData = outdoorGroup.FirstOrDefault(data => data.Datum == indoorData.Datum);
                    if (correspondingOutdoorData == null) continue;

                    if (indoorData.Temp < indoorGroup.Average(data => data.Temp) && correspondingOutdoorData.Temp > outdoorGroup.Average(data => data.Temp))
                    {
                        if (openStartTime == null)
                        {
                            openStartTime = indoorData.Datum;
                        }
                    }
                    else
                    {
                        if (openStartTime != null)
                        {
                            totalDuration += indoorData.Datum - openStartTime.Value;
                            openStartTime = null;
                        }
                    }
                }

                if (openStartTime != null)
                {
                    totalDuration += indoorGroup.Last().Datum - openStartTime.Value;
                }

                durations.Add((date, totalDuration));
            }

            // Sort by duration descending
            return durations.OrderByDescending(d => d.Duration).ToList();
        }
    }
}
