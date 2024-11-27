using EFCore.Models;
using EFTCore_Lab3.Models;
using System.Collections.Generic;


Console.WriteLine("Hello, World!");
//insertWeatherData();
updateWeatherData();
deleteWeatherData();

Console.WriteLine("Press any key to continue");
Console.ReadKey();

static void insertWeatherData()
{
    using (var db = new EFContext())
    {
        WeatherData weatherdata = new WeatherData();
        weatherdata.Plats = "Ute";
        db.Add(weatherdata);

        weatherdata = new WeatherData();
        weatherdata.Plats = "Inne";
        db.Add(weatherdata);

        db.SaveChanges();
    }
    return;
}



static void updateWeatherData()
{
    using (var db = new EFContext())
    {
        WeatherData weatherdata = db.WeatherData.Find(1);
        weatherdata.Plats = "Inne";
        db.SaveChanges();
    }
    return;

}

static void deleteWeatherData()
{
    using (var db = new EFContext())
    {
        WeatherData weatherData = db.WeatherData.Find(1);
        db.WeatherData.Remove(weatherData);
        db.SaveChanges();
    }
    return;

}