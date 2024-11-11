namespace DiscordBot.Services.Interfaces
{
    public interface IUserInputHandler
    {
        Task WaitForShutdownAsync();
    }
}