using Refit;
using Demo.WpfClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.WpfClient.Services
{
	[Headers("Ocp-Apim-Subscription-Key: 52a1d2a7fbf84ecb933a42001898decd")]
	public interface IApi1Service
	{
		[Get("/api1/WeatherForecast")]
		Task<IList<ForecastModel>> GetWeatherForecastAsync([Header("Authorization")] string authorization);
	}
}
