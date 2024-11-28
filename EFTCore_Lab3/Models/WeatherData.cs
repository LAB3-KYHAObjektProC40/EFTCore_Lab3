using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace WeatherDataApp.Models
{
    public class WeatherData
    {
        [Key]
        [Ignore] // Ignore this property during CSV mapping
        public int Id { get; set; }  // Primary key, auto-generated

        [Name("Datum")]
        public DateTime Datum { get; set; }   // Date and time of the measurement

        [Name("Plats")]
        public string Plats { get; set; }     // Location (e.g., "Ute", "Inne")

        [Name("Temp")]
        public float Temp { get; set; }       // Temperature in Celsius

        [Name("Luftfuktighet")]
        public int Luftfuktighet { get; set; } // Humidity percentage
    }
}
