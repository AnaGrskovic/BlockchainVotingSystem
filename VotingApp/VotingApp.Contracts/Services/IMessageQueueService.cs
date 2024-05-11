namespace VotingApp.Contracts.Services;

public interface IMessageQueueService
{
    Task SendMessageAsync(string message);
}
