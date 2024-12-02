using EFTCore_Lab3.Core.Utilities;
using EFTCore_Lab3.DataAccess;
using System;
using System.IO;

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
                        CsvImporter.ImportAndSaveCsvData();
                        break;

                    case "2a":
                        OutdoorDataProcessor.FetchOutdoorAverageTemperature();
                        break;
                    case "2b":
                        OutdoorDataProcessor.SortOutdoorDaysByTemperature();
                        break;
                    case "2c":
                        OutdoorDataProcessor.SortOutdoorDaysByHumidity();
                        break;
                    case "2d":
                        OutdoorDataProcessor.SortOutdoorDaysByMoldRisk();
                        break;
                    case "2e":
                        OutdoorDataProcessor.GetMeteorologicalAutumnDate();
                        break;
                    case "2f":
                        OutdoorDataProcessor.GetMeteorologicalWinterDate();
                        break;



                    case "3a":
                        IndoorDataProcessor.FetchIndoorAverageTemperature();
                        break;
                    case "3b":
                        IndoorDataProcessor.SortIndoorDaysByTemperature();
                        break;
                    case "3c":
                        IndoorDataProcessor.SortIndoorDaysByHumidity();
                        break;
                    case "3d":
                        var indoorData = CsvImporter.LoadIndoorData();
                        IndoorDataProcessor.SortIndoorDaysByMoldRisk(indoorData);
                        break;

                    case "4a":
                        BalconyDataProcessor.CalculateBalconyOpenTime();
                        break;
                    case "4b":
                        BalconyDataProcessor.SortDaysByTemperatureDifference();
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


    }
}