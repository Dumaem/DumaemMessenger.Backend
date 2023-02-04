using Messenger.Database.Models;
using Messenger.Domain.Models;
using Messenger.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMessageRepository _messageRepository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<Message> Get()
    {
        return await _messageRepository.GetMessageByIdAsync(2);
    }
}