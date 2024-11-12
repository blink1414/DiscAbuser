using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Core.Interfaces;
using DiscordBot.Services.Interfaces;

namespace DiscordBot.Services
{
    public class UserInputHandler : IUserInputHandler
    {
        private readonly IBot _bot;

        public UserInputHandler(IBot bot)
        {
            _bot = bot;
        }

        public async Task WaitForShutdownAsync()
        {
            Console.WriteLine("\r\nPress Q to shutdown");

            while (true)
            {
                var keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("\nShutting down!");
                    await _bot.StopAsync();
                    break;
                }
            }
        }
    }
}