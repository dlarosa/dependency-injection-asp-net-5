using System;

namespace DependencyInjection.Services
{
    public class WeatherService2 : IWeatherService
    {

        public string GetWeather(string city, string country)
        {
            var weather = "Heavy Snow";

            return string.Format("WeatherService2 \n\n{0} in {1} ({2})", weather, city, country);
        }
    }
}