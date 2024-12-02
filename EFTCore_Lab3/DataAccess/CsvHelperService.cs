using CsvHelper;
using CsvHelper.Configuration;
using EFTCore_Lab3.Core.Models;
using EFTCore_Lab3.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace EFTCore_Lab3.DataAccess
{
    public static class CsvHelperService
    {
        public static List<WeatherData> ReadCsvFile(string filePath)
        {
            try
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",                 // Use commas as delimiters
                    HasHeaderRecord = true,          // File has headers
                    IgnoreBlankLines = true,         // Skip blank lines
                    HeaderValidated = null,          // Disable header validation errors
                    BadDataFound = null              // Skip bad data without throwing an exception
                };

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    // Register custom float converter
                    csv.Context.TypeConverterCache.AddConverter<float>(new CustomFloatConverter());

                    return new List<WeatherData>(csv.GetRecords<WeatherData>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read the CSV file: {ex.Message}");
                throw;
            }
        }
    }
}
