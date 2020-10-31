using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JitBox.Api
{
  public class OpenWeatherResponse
  {
    public string Name { get; set; }

    [JsonPropertyName("weather")]
    public IEnumerable<WeatherDescription> Weather { get; set; }

    [JsonPropertyName("main")]
    public Main Main { get; set; }
  }
  public class WeatherDescription
  {
    [JsonPropertyName("main")]
    public string Main { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
  }


  public class Main
  {
    [JsonPropertyName("temp")]
    public double Temperature { get; set; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; set; }
  }
}
