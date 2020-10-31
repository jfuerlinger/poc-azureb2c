using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JitBox.Api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private const string xdkore = "41733a23f4510ec45dabd5d482e88457";

    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;

    // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
    static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

      var rng = new Random();
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
      })
      .ToArray();
    }

    // https://localhost:5001/weatherforecast/city/denver
    [HttpGet("[action]/{city}")]
    public async Task<IActionResult> City(string city)
    {
      using (var client = new HttpClient())
      {
        try
        {
          // api.openweathermap.org/data/2.5/weather?zip=92128&appid=4bc6abf258686a04166632d1357ab64e&units=imperial
          client.BaseAddress = new Uri("http://api.openweathermap.org");
          var response = await client.GetAsync($"/data/2.5/weather?q={city}&appid={xdkore}&units=imperial");
          response.EnsureSuccessStatusCode();

          var stringResult = await response.Content.ReadAsStringAsync();
          var rawWeather = JsonSerializer.Deserialize<OpenWeatherResponse>(stringResult);
          return Ok(new
          {
            Temperature = rawWeather.Main.Temperature,
            Summary = string.Join(",", rawWeather.Weather.Select(x => x.Main)),
            Humidity = rawWeather.Main.Humidity,
            City = rawWeather.Name
          });
        }
        catch (HttpRequestException httpRequestException)
        {
          return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
        }
      }
    }
  }
}
