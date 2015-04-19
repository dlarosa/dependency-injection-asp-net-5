using DependencyInjection.Services;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {

        IWeatherService _weatherService;
        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }


        [HttpGet("/")]
        public string Index()
        {
            return _weatherService.GetWeather("Miami", "US");
        }
    }
}
