using Discord;
using Discord.Commands;

namespace DiscordBot.Services.Interfaces
{
    public interface IFileHandler
    {
        Task EncryptAndSendFileAsync(IEnumerable<Attachment> file, SocketCommandContext context);
    }
}