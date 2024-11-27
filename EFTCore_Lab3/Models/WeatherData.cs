

using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace EFTCore_Lab3.Models
{

    [Keyless]
    public class WeatherData
    {
       
        //[Name("Datum")]
        public DateTime Datum { get; set; }   // Date and time of the measurement

        //[Name("Plats")]
        public string Plats { get; set; }  // Measurement location (e.g., "Ute", "Inne")

        //[Name("Temp")]
        public double Temp { get; set; }  // Temperature in Celsius

        //[Name("Luftfuktighet")]
        public int Luftfuktighet { get; set; } // Humidity in percentage

       


    }
}
