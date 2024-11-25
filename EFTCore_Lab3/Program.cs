using EFCore.Models;
using EFTCore_Lab3.Models;
using System.Collections.Generic;


Console.WriteLine("Hello, World!");
//insertWeatherData();
readWeatherData();
updateWeatherData();
readWeatherData();
deleteWeatherData();
readWeatherData();

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

static void readWeatherData()
{
    using(var db = new EFContext())
    {
        List<WeatherData> weatherdata = db.WeatherData.ToList();
        foreach (WeatherData p in weatherdata)
        {
            Console.WriteLine("{0} {1}", p.Id, p.Location);
        }
    }
    return;
}

static void updateWeatherData()
{
    using (var db = new EFContext())
    {
            WeatherData weatherdata = db.WeatherData.Find(1);
            weatherdata.Location = "Inne";
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