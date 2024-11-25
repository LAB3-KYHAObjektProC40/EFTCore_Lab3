

using System.Numerics;

namespace EFTCore_Lab3.Models
{
    public class WeatherData
    {
        public int Id { get; set; }  // Primary key
        public DateTime Timestamp { get; set; }   // Date and time of the measurement

        public string Location { get; set; }  // Measurement location (e.g., "Ute", "Inne")
        public double Temperature { get; set; }  // Temperature in Celsius

        public int Humidity { get; set; } // Humidity in percentage

       


    }
}
