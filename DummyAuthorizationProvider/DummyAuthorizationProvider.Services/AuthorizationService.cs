using DummyAuthorizationProvider.Contracts.Entities;
using DummyAuthorizationProvider.Contracts.Enums;
using DummyAuthorizationProvider.Contracts.Exceptions;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.UoW;

namespace DummyAuthorizationProvider.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUnitOfWork _uow;

    public AuthorizationService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<string> GetTokenAsync(string? oib)
    {
        if (oib == null)
        {
            throw new OibNotPresentException("Oib is not present in the request.");
        }
        List<Voter> voters = await GetAllAsync();
        Voter? voter = voters.FirstOrDefault(v => v.Oib.Equals(oib));
        if (voter == null)
        {
            throw new EntityNotFoundException("There is no voter with that oib.");
        }

        CheckIfVoteNothing(voter);

        int seed = int.Parse(oib);
        Random random = new Random(seed);
        return random.Next().ToString();
    }

    public async Task CheckTokenNothingAsync(string? token)
    {
        await CheckTokenAsync(token, VoterStatus.Nothing);
    }

    public async Task CheckTokenRequestedAsync(string? token)
    {
        await CheckTokenAsync(token, VoterStatus.VoteRequested);
    }

    public async Task SetVoteRequestedAsync(string? token)
    {
        Voter voter = await GetAsync(token) ?? throw new EntityNotFoundException("There is no voter with that token.");
        CheckIfVoteNothing(voter);
        voter.Status = VoterStatus.VoteRequested;
        _uow.Voters.Update(voter);
        await _uow.SaveChangesAsync();
    }

    public async Task SetVoteCreatedAsync(string? token)
    {
        Voter voter = await GetAsync(token) ?? throw new EntityNotFoundException("There is no voter with that token.");
        CheckIfVoteRequested(voter);
        voter.Status = VoterStatus.VoteCreated;
        _uow.Voters.Update(voter);
        await _uow.SaveChangesAsync();
    }

    private async Task<List<Voter>> GetAllAsync()
    {
        return await _uow.Voters.GetAllAsync();
    }

    private async Task CheckTokenAsync(string? token, VoterStatus voterStatus)
    {
        bool isTokenValid = await IsTokenValidAsync(token, voterStatus);
        if (!isTokenValid)
        {
            throw new TokenNotValidException("Token is not valid.");
        }
    }

    private void CheckIfVoteNothing(Voter voter)
    {
        if (voter.Status != VoterStatus.Nothing)
        {
            throw new VoterAlreadyVotedException("Voter has already voted.");
        }
    }

    private void CheckIfVoteRequested(Voter voter)
    {
        if (voter.Status != VoterStatus.VoteRequested)
        {
            throw new VoterAlreadyVotedException("Voter has already voted.");
        }
    }

    private async Task<bool> IsTokenValidAsync(string? token, VoterStatus voterStatus)
    {
        Voter? voter = await GetAsync(token);
        if (voter == null)
        {
            return false;
        }

        if (voter.Status != voterStatus)
        {
            return false;
        }

        return true;
    }

    private async Task<Voter?> GetAsync(string? token)
    {
        if (token == null)
        {
            throw new TokenNotPresentException("Token is not present in the request.");
        }
        List<Voter> voters = await GetAllAsync();
        foreach (Voter voter in voters)
        {
            int seed = int.Parse(voter.Oib);
            Random random = new Random(seed);
            if (random.Next().ToString().Equals(token))
            {
                return voter;
            }
        }
        return null;
    }
}
