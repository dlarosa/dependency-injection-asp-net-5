using System;

namespace DependencyInjection.Services
{
    public interface IWeatherService
    {
        string GetWeather(string city, string country);
    }
}