using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;

namespace EFTCore_Lab3.Utilities
{
    public class CustomDecimalConverter : DecimalConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Trim whitespace and replace non-standard minus signs with the correct minus
            text = text?.Trim();

            // Replace Unicode minus (U+2212) with ASCII hyphen-minus (U+002D)
            if (text != null)
            {
                text = text.Replace('−', '-'); // Fix possible non-standard minus signs
            }

            // Attempt to parse the value as a decimal using InvariantCulture
            if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result; // Successfully parsed value
            }

            // If parsing fails, we can decide whether to:
            // 1. Throw an exception (as done here).
            // 2. Default to a specific value like 0.0m.

            throw new FormatException($"Unable to parse '{text}' as a decimal on row {row.Context.Parser.Row}");
        }
    }
}
