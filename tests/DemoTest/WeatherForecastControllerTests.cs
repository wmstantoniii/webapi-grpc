using Microsoft.Extensions.Logging;
using Moq;
using webapi.Controllers;

namespace TestProject1;

public class WeatherForecastControllerTests
{
    [Fact]
    public void WeatherForecastsAreWellFormed()
    {
        var logger = new Mock<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger.Object);
        var forecasts = controller.Get();
        Assert.True(forecasts.Any());
        Assert.True(forecasts.All(forecast => forecast is not null)); 
        Assert.True(forecasts.All(forecast => forecast.Date >= DateTime.Now));
        Assert.True(forecasts.All(forecast => !string.IsNullOrEmpty(forecast.Summary)));
        Assert.True(forecasts.All(forecast => forecast.TemperatureC >= -20));
   }
}