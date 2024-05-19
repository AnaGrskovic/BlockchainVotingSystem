using Azure;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class VotingService : IVotingService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICandidateService _candidateService;
    private readonly IBackupService _backupService;
    private readonly IMessageQueueService _messageQueueService;

    public VotingService(
        IAuthorizationService authorizationService,
        ICandidateService candidateService,
        IBackupService backupService,
        IMessageQueueService messageQueueService)
    {
        _authorizationService = authorizationService;
        _candidateService = candidateService;
        _backupService = backupService;
        _messageQueueService = messageQueueService;
    }

    public async Task VoteAsync(string? token, string? vote)
    {
        if (token is null)
        {
            throw new TokenNotPresentException("Token is not present in the request.");
        }
        bool isTokenValid = await _authorizationService.CheckTokenAsync(token);
        if (!isTokenValid)
        {
            throw new TokenNotValidException("Token is not valid.");
        }
        if (vote is null)
        {
            throw new VoteNotPresentException("Vote is not present in the request.");
        }

        var isCandidateValid = _candidateService.Check(vote);
        if (!isCandidateValid)
        {
            throw new CandidateNotValidException("Candidate is not valid.");
        }

        await _backupService.CreateAsync(vote);

        var voteDto = new VoteDto(token, vote);
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var voteMessage = JsonSerializer.Serialize(voteDto, serializeOptions);
        await _messageQueueService.SendMessageAsync(voteMessage);

        await _authorizationService.SetVotedAsync(token);
    }
}
