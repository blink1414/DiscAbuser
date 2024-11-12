using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Core.Interfaces
{
    public interface IBot
    {
        Task StartAsync();

        Task StopAsync();

    }
}