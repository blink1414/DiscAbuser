using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Services
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider serviceProvider)
        {
            _client = client;
            _commands = commands;
            _serviceProvider = serviceProvider;
        }

        public async Task HandleCommandAsync(SocketMessage arg)
        {
            if (arg is not SocketUserMessage message || message.Author.IsBot)
                return;

            int position = 0;
            if (message.HasCharPrefix('!', ref position))
            {
                var context = new SocketCommandContext(_client, message);
                await _commands.ExecuteAsync(context, position, _serviceProvider);
            }
        }
    }
}