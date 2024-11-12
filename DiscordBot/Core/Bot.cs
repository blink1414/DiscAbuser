using Discord.Commands;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using DiscordBot.Services;
using DiscordBot.Services.Interfaces;
using DiscordBot.Core.Interfaces;

public class Bot : IBot
{
    private readonly IConfiguration _configuration;
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly ICommandHandler _commandHandler;

    public Bot(IConfiguration configuration, DiscordSocketClient client, CommandService commands, CommandHandler commandHandler)
    {
        _configuration = configuration;
        _client = client;
        _commands = commands;
        _commandHandler = commandHandler;

        _client.Ready += OnBotReadyAsync;
        _client.MessageReceived += _commandHandler.HandleCommandAsync;

    }

    public async Task StartAsync()
    {
        string discordToken = _configuration["DiscordToken"] ?? throw new Exception("Missing Discord token");

        await _client.LoginAsync(TokenType.Bot, discordToken);
        await _client.StartAsync();

        Console.WriteLine("Connected to Discord");
    }

    private async Task OnBotReadyAsync()
    {
        Console.WriteLine($"\r\n\r\n\r\n{DateTime.Now} -----> Bot is ready to go!");

        ulong guildId = 1236048592599253054;
        ulong channelId = 1236048594407133196;

        var guild = _client.GetGuild(guildId);
        if (guild == null)
        {
            Console.WriteLine("Guild not found.");
            return;
        }

        var channel = guild.GetTextChannel(channelId);
        if (channel == null)
        {
            Console.WriteLine("Channel not found.");
            return;
        }

        await channel.SendMessageAsync("I'm in master");
    }

    public async Task StopAsync()
    {
        await _client.LogoutAsync();
        await _client.StopAsync();
        await _client.DisposeAsync();
    }
}
