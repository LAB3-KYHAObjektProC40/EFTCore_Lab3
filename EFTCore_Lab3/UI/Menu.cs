using EFTCore_Lab3.Core.Utilities;
using EFTCore_Lab3.DataAccess;
using System;

namespace EFTCore_Lab3.UI
{
    public class Menu
    {
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear(); // Clear the console for better readability

                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("|                               WEATHER DATA MENU                                    |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("|                                Menu options:                                       |");
                Console.WriteLine("|                                ex = 3a                                             |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("|                                Date between:                                       |");
                Console.WriteLine("|                                2016-10-01 - 2016-11-30                             |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("| 1. Läs in och spara CSV-data                                                       |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("| 2. Hämta och analysera Utomhus Data                                                |");
                Console.WriteLine("|    a) Medeltemperatur för valt datum                                               |");
                Console.WriteLine("|    b) Sortering: Varmaste till kallaste dagen                                      |");
                Console.WriteLine("|    c) Sortering: Torraste till fuktigaste dag                                      |");
                Console.WriteLine("|    d) Sortering: Minst till störst mögelrisk                                       |");
                Console.WriteLine("|    e) Datum för meteorologisk Höst                                                 |");
                Console.WriteLine("|    f) Datum för meteorologisk Vinter                                               |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("| 3. Hämta och analysera Inomhus Data                                                |");
                Console.WriteLine("|    a) Medeltemperatur för valt datum                                               |");
                Console.WriteLine("|    b) Sortering av varmaste till kallaste dagen enligt medeltemperatur per dag     |");
                Console.WriteLine("|    c) Sortering av torraste till fuktigaste dagen enligt medelluftfuktighet per dag|");
                Console.WriteLine("|    d) Sortering av minst till störst risk för mögel                                |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("| 4. Hämta och analysera Balkongdörren Data                                          |");
                Console.WriteLine("| a) Hur länge är balkongdörren öppen per dag, och sortera på detta                  |");
                Console.WriteLine("| b) Sortering på då inne- och utetemperaturerna skiljt sig mest och minst           |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("| 5. Avsluta                                                                         |");
                Console.WriteLine("|------------------------------------------------------------------------------------|");
                Console.WriteLine("\n");

                Console.Write("Välj ett alternativ: ");
                string? choice = Console.ReadLine()?.Trim();

                Console.WriteLine("\n");

                switch (choice)
                {
                    case "1":
                        ImportAndSaveCsvData();
                        break;

                    case "2a":
                        FetchOutdoorAverageTemperature();
                        break;
                    case "2b":
                        SortOutdoorDaysByTemperature();
                        break;
                    case "2c":
                        SortOutdoorDaysByHumidity();
                        break;
                    case "2d":
                        SortOutdoorDaysByMoldRisk();
                        break;
                    case "2e":
                        GetMeteorologicalAutumnDate();
                        break;
                    case "2f":
                        GetMeteorologicalWinterDate();
                        break;

                    case "3a":
                        FetchIndoorAverageTemperature();
                        break;
                    case "3b":
                        SortIndoorDaysByTemperature();
                        break;
                    case "3c":
                        SortIndoorDaysByHumidity();
                        break;
                    case "3d":
                        SortIndoorDaysByMoldRisk();
                        break;

                    case "4a":
                        CalculateBalconyOpenTime();
                        break;
                    case "4b":
                        SortDaysByTemperatureDifference();
                        break;
                    case "5":
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

                Console.WriteLine("\nTryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }

        // Method implementations for CSV import
        public void ImportAndSaveCsvData()
        {
            // Dynamically calculate the path to the Resources folder
            var csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "weatherdata.csv");

            Console.WriteLine($"Looking for file at: {csvFilePath}");

            if (!File.Exists(csvFilePath))
            {
                Console.WriteLine("CSV file not found. Please ensure it exists in the 'Resources' folder.");
                return;
            }

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


        private void FetchOutdoorAverageTemperature()
        {
            Console.Write("Ange ett datum (YYYY-MM-DD): ");
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out var specificDate))
            {
                using (var db = new EFContext())
                {
                    var allWeatherData = db.WeatherData.ToList();
                    var utomhus = new Utomhus(allWeatherData);

                    try
                    {
                        var avgTemp = utomhus.GetAverageTemperature(specificDate);
                        Console.WriteLine($"Medeltemperatur för {specificDate:yyyy-MM-dd}: {avgTemp}°C");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt datumformat.");
            }
        }

        private void SortOutdoorDaysByTemperature()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByTemperature = utomhus.GetDaysSortedByTemperature();
                Console.WriteLine("Dagar sorterade efter medeltemperatur (varmast till kallast):");
                foreach (var (date, avgTemp) in sortedByTemperature)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgTemp}°C");
                }
            }
        }

        private void SortOutdoorDaysByHumidity()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByHumidity = utomhus.GetDaysSortedByHumidity();
                Console.WriteLine("Dagar sorterade efter medelluftfuktighet (torrast till fuktigast):");
                foreach (var (date, avgHumidity) in sortedByHumidity)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgHumidity}%");
                }
            }
        }

        private void SortOutdoorDaysByMoldRisk()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var sortedByMoldRisk = utomhus.GetDaysSortedByMoldRisk();
                Console.WriteLine("Dagar sorterade efter mögelrisk (minst till störst):");
                foreach (var (date, moldRisk) in sortedByMoldRisk)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {moldRisk:F2}");
                }
            }
        }
        private void GetMeteorologicalAutumnDate()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var autumnDate = utomhus.GetDateForAutumn();
                if (autumnDate.HasValue)
                    Console.WriteLine($"Datum för meteorologisk Höst: {autumnDate.Value:yyyy-MM-dd}");
                else
                    Console.WriteLine("Ingen meteorologisk Höst hittades.");
            }
        }

        private void GetMeteorologicalWinterDate()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var utomhus = new Utomhus(allWeatherData);

                var winterDate = utomhus.GetDateForWinter();
                if (winterDate.HasValue)
                    Console.WriteLine($"Datum för meteorologisk Vinter: {winterDate.Value:yyyy-MM-dd}");
                else
                    Console.WriteLine("Ingen meteorologisk Vinter hittades.");
            }
        }


        private void FetchIndoorAverageTemperature()
        {
            Console.Write("Ange ett datum (YYYY-MM-DD): ");
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out var specificDate))
            {
                using (var db = new EFContext())
                {
                    var allWeatherData = db.WeatherData.ToList();
                    var inomhus = new Inomhus(allWeatherData);

                    try
                    {
                        var avgTemp = inomhus.GetAverageTemperature(specificDate);
                        Console.WriteLine($"Medeltemperatur för {specificDate:yyyy-MM-dd}: {avgTemp}°C");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt datumformat.");
            }
        }

        private void SortIndoorDaysByTemperature()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByTemperature = inomhus.GetDaysSortedByTemperature();
                Console.WriteLine("Inomhus dagar sorterade efter medeltemperatur (varmast till kallast):");
                foreach (var (date, avgTemp) in sortedByTemperature)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgTemp}°C");
                }
            }
        }

        private void SortIndoorDaysByHumidity()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByHumidity = inomhus.GetDaysSortedByHumidity();
                Console.WriteLine("Inomhus dagar sorterade efter medelluftfuktighet (torrast till fuktigast):");
                foreach (var (date, avgHumidity) in sortedByHumidity)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {avgHumidity}%");
                }
            }
        }

        private void SortIndoorDaysByMoldRisk()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var inomhus = new Inomhus(allWeatherData);

                var sortedByMoldRisk = inomhus.GetDaysSortedByMoldRisk();
                Console.WriteLine("Inomhus dagar sorterade efter mögelrisk (minst till störst):");
                foreach (var (date, moldRisk) in sortedByMoldRisk)
                {
                    Console.WriteLine($"{date:yyyy-MM-dd}: {moldRisk:F2}");
                }
            }
        }

        private void CalculateBalconyOpenTime()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var analysis = new TemperatureDifferenceAnalysis(allWeatherData);

                var durations = analysis.CalculateBalconyDoorOpenDuration();

                Console.WriteLine("Balkongdörrens öppettider per dag (sorterat efter längd):");
                foreach (var (Date, Duration) in durations)
                {
                    Console.WriteLine($"{Date:yyyy-MM-dd}: {Duration.TotalHours:F2} timmar");
                }
            }
        }






        private void SortDaysByTemperatureDifference()
        {
            using (var db = new EFContext())
            {
                var allWeatherData = db.WeatherData.ToList();
                var analysis = new TemperatureDifferenceAnalysis(allWeatherData);

                var differences = analysis.SortByTemperatureDifference();

                Console.WriteLine("Dagar sorterade efter temperaturskillnad (störst till minst):");
                foreach (var (Date, TemperatureDifference) in differences)
                {
                    Console.WriteLine($"{Date:yyyy-MM-dd}: {TemperatureDifference:F2}°C");
                }
            }
        }





    }
}