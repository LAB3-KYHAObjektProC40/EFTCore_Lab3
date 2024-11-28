using System;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace EFTCore_Lab3.Models
{
    public class WeatherData
    {
        [Key]
        public int Id { get; set; }  // Primary key, auto-generated

        [Name("Datum")]
        public DateTime Datum { get; set; }   // Date and time of the measurement

        [Name("Plats")]
        public string Plats { get; set; }  // Measurement location (e.g., "Ute", "Inne")

        [Name("Temp")]
        public decimal Temp { get; set; }  // Temperature in Celsius

        [Name("Luftfuktighet")]
        public int Luftfuktighet { get; set; } // Humidity in percentage
    }
}
