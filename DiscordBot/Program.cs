using DiscordBot.Core.Interfaces;
using DiscordBot.Services;
using DiscordBot.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using var serviceProvider = ServiceConfigurator.ConfigureServices();
        var bot = serviceProvider.GetRequiredService<IBot>();
        var userInputHandler = serviceProvider.GetRequiredService<IUserInputHandler>();

        try
        {
            await bot.StartAsync();

            await userInputHandler.WaitForShutdownAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Environment.Exit(-1);
        }
    }
}