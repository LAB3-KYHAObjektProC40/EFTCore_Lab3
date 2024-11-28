using EFTCore_Lab3.Utilities;
using System;

namespace EFTCore_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvFilePath = @"C:\Users\Simon Major\source\repos\EFTCore_Lab3_Solution\EFTCore_Lab3\weatherdata.csv"; // Update with the actual CSV file path

            try
            {
                // Step 1: Read CSV file
                var weatherData = CsvHelperService.ReadCsvFile(csvFilePath);

                // Step 2: Save data to the database
                DatabaseHelper.SaveToDatabase(weatherData);

                Console.WriteLine($"{weatherData.Count} records successfully saved to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
