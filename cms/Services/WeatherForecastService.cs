using Cms.Models;


namespace Cms.Services;


public class WeatherForecastService : IWeatherForecastService
{
	#region Members
	private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
	#endregion


	#region Public methods
	public IEnumerable<WeatherForecast> GetForecasts(int days) => Enumerable.Range(1, days).Select(index =>
		new WeatherForecast
		(
			DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
			Random.Shared.Next(-20, 55),
			Summaries[Random.Shared.Next(Summaries.Length)]
		))
		.ToArray();


	public Task AddForecastAsync(WeatherForecast forecast)
	{
		throw new NotImplementedException();
	}
	#endregion
}