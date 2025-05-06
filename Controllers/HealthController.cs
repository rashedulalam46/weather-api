using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _weatherApiKey;

    public HealthController(IHttpClientFactory httpClientFactory, IOptions<WeatherApiOptions> options)
    {
        _httpClient = httpClientFactory.CreateClient();
        _weatherApiKey = options.Value.Key;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string url = $"https://api.weatherapi.com/v1/current.json?key={_weatherApiKey}&q=Dhaka";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(503, new
                {
                    status = "unhealthy",
                    weatherApiStatus = $"Down - Status code: {response.StatusCode}"
                });
            }

            return Ok(new
            {
                status = "healthy",
                weatherApiStatus = "reachable"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(503, new
            {
                status = "unhealthy",
                weatherApiStatus = "unreachable",
                error = ex.Message
            });
        }
    }
}
