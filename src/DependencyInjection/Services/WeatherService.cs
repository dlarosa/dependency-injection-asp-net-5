using System;
using WeatherNet.Model;

namespace DependencyInjection.Services
{
    public class WeatherService :IWeatherService
    {
       
        public string GetWeather(string city, string country)
        {
            var wClient = new WeatherNet.Clients.CurrentWeather();
            var weather = wClient.GetByCityName(city, country).Item.Description;

            return string.Format("WeatherService \n\n{0} in {1} ({2})", weather, city, country);
        }

    }
}