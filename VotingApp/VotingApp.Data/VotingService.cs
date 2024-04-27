using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class VotingService : IVotingService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMessageQueueService _messageQueueService;

    public VotingService(
        IAuthorizationService authorizationService,
        IMessageQueueService messageQueueService)
    {
        _authorizationService = authorizationService;
        _messageQueueService = messageQueueService;
    }

    public async Task VoteAsync(string? token, string? vote)
    {
        if (token is null)
        {
            throw new TokenNotPresentException("Token not present in the request.");
        }
        bool isTokenValid = await _authorizationService.CheckTokenAsync(token);
        if (!isTokenValid)
        {
            throw new TokenNotValidException("Token is not valid.");
        }
        if (vote is null)
        {
            throw new VoteNotPresentException("Vote not present in the request.");
        }
        _messageQueueService.SendMessage(vote);
    }
}
