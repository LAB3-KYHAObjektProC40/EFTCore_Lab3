using CsvHelper.Configuration;
using EFTCore_Lab3.Models;

namespace EFTCore_Lab3.Utilities
{
    public class WeatherDataMap : ClassMap<WeatherData>
    {
        public WeatherDataMap()
        {
            // Explicitly map the CSV columns to properties
            Map(m => m.Datum).Name("Datum");
            Map(m => m.Plats).Name("Plats");
            Map(m => m.Temp).Name("Temp");
            Map(m => m.Luftfuktighet).Name("Luftfuktighet");

            // Ignore the 'Id' property since it's not in the CSV
            Map(m => m.Id).Ignore();
        }
    }
}
