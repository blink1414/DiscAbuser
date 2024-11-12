using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Services;

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
        var context = new SocketCommandContext(_client, message);

        if (!message.HasCharPrefix('!', ref position))
        {
            return;
        }

        var command = message.Content.Substring(1).Trim(); 

        if (command.StartsWith("echo", StringComparison.OrdinalIgnoreCase))
        {
            var echoContent = command.Length > 5 ? command.Substring(5).Trim() : string.Empty; 
            await context.Channel.SendMessageAsync($"Echo: {echoContent}");
        }
        else
        {
            await context.Channel.SendMessageAsync("Sorry, I do not have any commands, only [ !echo <phrase> ].");
        }
    }

    //[Command("echo")]
    //[Summary("Echoes back what was said")]
    //private async Task<string> EchoCommand([Summary("A phrase")] string message)
    //{
    //    return message;
    //}

    // criar classe de commandos

}