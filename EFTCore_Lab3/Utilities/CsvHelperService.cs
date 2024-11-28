using CsvHelper;
using CsvHelper.Configuration;
using EFTCore_Lab3.Models;
using EFTCore_Lab3.Utilities;  // Import mapping class and converter
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EFTCore_Lab3.Utilities
{
    public static class CsvHelperService
    {
        public static List<WeatherData> ReadCsvFile(string filePath)
        {
            try
            {
                // Setting CultureInfo to Swedish (or any culture that matches your CSV data format)
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("sv-SE"))
                {
                    Delimiter = ",",                 // Use commas as delimiters
                    HasHeaderRecord = true,          // File has headers
                    IgnoreBlankLines = true,         // Skip blank lines
                    HeaderValidated = null           // Disable header validation errors
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    // Register the custom mapping class
                    csv.Context.RegisterClassMap<WeatherDataMap>();

                    // Read and map records directly to the WeatherData model
                    return new List<WeatherData>(csv.GetRecords<WeatherData>());
                }
            }
            catch (HeaderValidationException headerEx)
            {
                Console.WriteLine($"Header validation failed: {headerEx.Message}");
                throw;
            }
            catch (CsvHelperException csvEx)
            {
                Console.WriteLine($"CSV parsing error: {csvEx.Message}");
                if (csvEx.Context != null && csvEx.Context.Parser != null)
                {
                    Console.WriteLine($"Error at row: {csvEx.Context.Parser.Row}");
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while reading the CSV file: {ex.Message}");
                throw;
            }
        }
    }
}
