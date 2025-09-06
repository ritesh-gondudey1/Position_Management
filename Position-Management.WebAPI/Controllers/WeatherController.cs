using Microsoft.AspNetCore.Mvc;

namespace Position_Management.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWeather()
        {
            var weatherInfo = new
            {
                Temperature = 25,
                Condition = "Sunny",
                Location = "New York",
                Date = DateTime.Now
            };
            return Ok(weatherInfo);
        }
    }
}