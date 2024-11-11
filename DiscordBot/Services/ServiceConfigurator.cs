using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Interfaces;
using DiscordBot.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiscordBot.Services
{
    public static class ServiceConfigurator
    {
        public static ServiceProvider ConfigureServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var discordConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };

            return new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(discordConfig)
                .AddSingleton(provider =>
                    new DiscordSocketClient(provider.GetRequiredService<DiscordSocketConfig>()))
                .AddSingleton<CommandService>()
                .AddScoped<CommandHandler>()
                .AddScoped<IBot, Bot>()
                .AddScoped<IUserInputHandler, UserInputHandler>()
                .AddScoped<ICommandHandler, CommandHandler>()
                .BuildServiceProvider();
        }
    }
}