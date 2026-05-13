using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Models;
using Cms.Infrastructure.Services;


namespace Cms.Controllers;


[Authorize]
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