using System;
using System.Collections.Generic;
using System.Linq;
using EFTCore_Lab3.Core.Models;

namespace EFTCore_Lab3.Core.Utilities
{
    public class BalconyDoorAnalysis
    {
        private readonly List<WeatherData> _indoorData;
        private readonly List<WeatherData> _outdoorData;
        private const float Threshold = 1.0f; // Fixed threshold for temperature difference

        public BalconyDoorAnalysis(List<WeatherData> weatherData)
        {
            _indoorData = weatherData.Where(data => data.Plats.Equals("Inne", StringComparison.OrdinalIgnoreCase)).ToList();
            _outdoorData = weatherData.Where(data => data.Plats.Equals("Ute", StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<(DateTime Date, double DurationInHours)> CalculateBalconyDoorOpenDuration()
        {
            var durations = new List<(DateTime Date, double DurationInHours)>();

            var groupedIndoor = _indoorData.GroupBy(data => data.Datum.Date);
            var groupedOutdoor = _outdoorData.GroupBy(data => data.Datum.Date);

            foreach (var indoorGroup in groupedIndoor)
            {
                var date = indoorGroup.Key;
                var outdoorGroup = groupedOutdoor.FirstOrDefault(g => g.Key == date);

                if (outdoorGroup == null)
                {
                    durations.Add((date, 0));
                    continue;
                }

                double openDuration = 0;

                foreach (var indoorData in indoorGroup)
                {
                    var matchingOutdoor = outdoorGroup.FirstOrDefault(outdoorData =>
                        outdoorData.Datum.Hour == indoorData.Datum.Hour &&
                        outdoorData.Datum.Minute == indoorData.Datum.Minute);

                    if (matchingOutdoor != null && indoorData.Temp + Threshold < matchingOutdoor.Temp)
                    {
                        openDuration += 1.0 / 60; // Add 1 minute
                    }
                }

                durations.Add((date, openDuration));
            }

            return durations.OrderByDescending(d => d.DurationInHours).ToList();
        }
    }
}
