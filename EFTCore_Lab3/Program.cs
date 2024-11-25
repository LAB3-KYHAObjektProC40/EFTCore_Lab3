using EFCore.Models;
using EFTCore_Lab3.Models;


Console.WriteLine("Hello, World!");
insertWeatherData();

Console.WriteLine("Press any key to continue");
Console.ReadKey();

static void insertWeatherData()
{
    using(var db = new EFContext())
    {
        WeatherData weatherdata = new WeatherData();
        weatherdata.Location = "Ute";
        db.Add(weatherdata);

        weatherdata = new WeatherData();
        weatherdata.Location = "Inne";
        db.Add(weatherdata);

        db.SaveChanges();
    }
    return;
}