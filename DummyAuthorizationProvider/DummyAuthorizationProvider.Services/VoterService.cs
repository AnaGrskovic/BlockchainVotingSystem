using DummyAuthorizationProvider.Contracts.Entities;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.UoW;

namespace DummyAuthorizationProvider.Services;

public class VoterService : IVoterService
{
    private readonly IUnitOfWork _uow;

    public VoterService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public string GetToken(string oib)
    {
        Random random = new Random(oib.GetHashCode());
        return random.Next().ToString();
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        List<Voter> voters = await GetAllAsync();
        foreach (Voter voter in voters)
        {
            Random random = new Random(voter.Oib.GetHashCode());
            if (random.Next().ToString().Equals(token))
            {
                return true;
            }
        }
        return false;
    }

    private async Task<List<Voter>> GetAllAsync()
    {
        return await _uow.Voters.GetAllAsync();
    }
}
