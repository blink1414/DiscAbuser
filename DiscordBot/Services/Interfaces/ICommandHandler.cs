using Discord.WebSocket;

namespace DiscordBot.Services.Interfaces
{
    public interface ICommandHandler
    {
        Task HandleCommandAsync(SocketMessage arg);
    }
}