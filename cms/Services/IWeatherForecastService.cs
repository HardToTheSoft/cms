using Cms.Models;


namespace Cms.Services;


public interface IWeatherForecastService
{
    IEnumerable<WeatherForecast> GetForecasts(int days);
		
    Task AddForecastAsync(WeatherForecast forecast);
}