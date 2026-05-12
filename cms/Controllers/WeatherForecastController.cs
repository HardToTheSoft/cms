using Microsoft.AspNetCore.Mvc;

using Cms.Models;


namespace Cms.Controllers;


[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
	#region Members
	private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
	#endregion


	#region Actions
	[HttpGet]
	public IEnumerable<WeatherForecast> Get() => Enumerable.Range(1, 5).Select(index =>
		new WeatherForecast
		(
				DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				Random.Shared.Next(-20, 55),
				Summaries[Random.Shared.Next(Summaries.Length)]
		))
		.ToArray();
	#endregion
}