namespace VotingApp.Contracts.Services;

public interface IMessageQueueService
{
    void SendMessage(string message);
    Task SendMessageAsync(string message);
}
