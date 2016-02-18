# Dependency Injection in ASP.NET 5

Simple example of how to use dependency injection in an ASP.NET 5 project.

###Introduction

The following lines show how to inject a service in an ASP.NET 5 controller. The application created displays the weather forecast and will be able to use two different services: WeatherService, which uses [WeatherNet](https://www.nuget.org/packages/WeatherNet/ "WeatherNet") to provide real weather forecast, and WeatherService2, which as you will see later, it is not a very reliable forecast provider. 

###Creating the service

The first thing we need to do is to create an interface that our services can implement. This interface will only have one method, which will provide the weather forecast based on a location.
 
 
```csharp

public interface IWeatherService
{
    string GetWeather(string city, string country);
}
```

Once we have the interface, we create a new class that will contain the service implementation. This service we will retrieve and return the weather forecast using WeatherNet, based on the city and country.

```csharp
public class WeatherService :IWeatherService
{

    public string GetWeather(string city, string country)
    {
        var wClient = new WeatherNet.Clients.CurrentWeather();
        var weather = wClient.GetByCityName(city, country).Item.Description;
        
        return string.Format("WeatherService \n\n{0} in {1} ({2})", weather, city, country);
    }
}
```

Now, we add the WeatherService2 implementation in a new class. This service will not retrieve the weather using any third party. Instead, it will make the assumption that there is heavy snow everywhere. A little bit scary, right? ;)

```csharp
public class WeatherService2 : IWeatherService
{

    public string GetWeather(string city, string country)
    {
        var weather = "Heavy Snow";
        
        return string.Format("WeatherService2 \n\n{0} in {1} ({2})", weather, city, country);
    }
}
```

In our sample application, I put the three classes in three different files under a Service folder created in the root folder.

<p align="center">
  <img src="https://raw.githubusercontent.com/dlarosa/dependency-injection-asp-net-5/master/images/1.PNG?raw=true" alt=""/>
 </p>

###Injecting the service into the controller
 
It's time to inject the service to our Controller. To do this, we create a Controller constructor, which is going to take an IWeatherService implementation as a parameter. We also create a private field so that it's available to the rest of the methods of the Controller.
 
 ```csharp
IWeatherService _weatherService;
public HomeController(IWeatherService weatherService)
{
     _weatherService = weatherService;
}
 ```


Now we want to use the injected service in the Index method of our Controller to return the weather forecast. To keep it simple, the parameters will be hard coded in this method.


```csharp
[HttpGet("/")]
public string Index()
{
    return _weatherService.GetWeather("Miami", "US");
}
```

 

###Registering the service
 
Now let's do some magic and use the new dependency injection feature that comes with ASP.NET 5.

In the Startup.cs file we have a ConfigureService method, which we need to add the following line to register our service in the application:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
    services.AddInstance<IWeatherService>(new WeatherService());
}
```
            
This line means that every time our app needs an instance of IWeatherService, the WeatherService implementation will be used. 

Notice that we have used the AddInstance method to register our service, this is going to provide the same service instance for each HTTP request that uses the service. 

However in some cases we may want to create a new instance every time that the service is used. In this case, we can use something like the AddTransient method, which will provide a new instance of the service every time it's needed:

```csharp
services.AddTransient<IWeatherService>(x=> new WeatherService());
```



###Running the application!

We now run our application and observe how, as expected, the sky is clear in Miami. This seems right to me, and means that our WeatherService implementation was used.



<p align="center">
  <img src="https://raw.githubusercontent.com/dlarosa/dependency-injection-asp-net-5/master/images/2.PNG?raw=true" alt=""/>
 </p>


If, for any reason, we want to use WeatherService2, we just need to modify the ConfigureServices method.

```csharp
public void ConfigureServices(IServiceCollection services)
{
      services.AddMvc();
      services.AddInstance<IWeatherService>(new WeatherService2());
}
```

Let's run our application again, and voilà. Heavy snow in Miami, which means that this time our application used the WeatherService2 implementation.

 <p align="center">
  <img src="https://raw.githubusercontent.com/dlarosa/dependency-injection-asp-net-5/master/images/3.PNG?raw=true" alt=""/>
 </p>





