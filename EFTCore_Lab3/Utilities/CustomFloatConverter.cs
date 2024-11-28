using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace WeatherDataApp.Utilities
{
    public class CustomFloatConverter : SingleConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Trim whitespace and normalize minus signs
            text = text?.Trim()?.Replace('−', '-'); // Replace non-standard minus signs with ASCII hyphen

            if (float.TryParse(text, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out float result))
            {
                return result; // Successfully parsed value
            }

            // Log error and throw exception for invalid values
            throw new FormatException($"Unable to parse '{text}' as a float on row {row.Context.Parser.Row}.");
        }
    }
}
