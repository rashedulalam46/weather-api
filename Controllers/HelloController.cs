using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public HelloController(IHttpClientFactory httpClientFactory, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiKey = options.Value.Key;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string url = $"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q=Dhaka";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, "Failed to fetch weather data.");

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var temp = doc.RootElement.GetProperty("current").GetProperty("temp_c").ToString();

        var result = new
        {
            hostname = "server1",
            datetime = DateTime.Now.ToString("yyMMddHHmm"),
            version = "1.0.0",
            weather = new
            {
                dhaka = new
                {
                    temperature = temp,
                    temp_unit = "c"
                }
            }
        };

        return Ok(result);
    }
}
