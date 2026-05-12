using Microsoft.AspNetCore.Mvc;

using Cms.Models;
using Cms.Services;


namespace Cms.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
	#region Members
	private readonly IWeatherForecastService _weatherForecastService;
	#endregion


	#region Constructor
	public WeatherForecastController(IWeatherForecastService weatherForecastService)
	{
		_weatherForecastService = weatherForecastService;
	}
	#endregion


	#region Actions
	[HttpGet]
	public IEnumerable<WeatherForecast> Get([FromQuery] int days = 5) => _weatherForecastService.GetForecasts(days);
	#endregion
}