using DummyAuthorizationProvider.Contracts.Entities;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.UoW;
using System.Numerics;
using System.Security.Cryptography;

namespace DummyAuthorizationProvider.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUnitOfWork _uow;

    public AuthorizationService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<string?> GetTokenAsync(string oib)
    {
        List<Voter> voters = await GetAllAsync();
        Voter? voter = voters.FirstOrDefault(v => v.Oib.Equals(oib));
        if (voter == null)
        {
            return null;
        }
        int seed = int.Parse(oib);
        Random random = new Random(seed);
        return random.Next().ToString();
    }

    public async Task<bool> IsTokenValidAsync(string token)
    {
        List<Voter> voters = await GetAllAsync();
        foreach (Voter voter in voters)
        {
            int seed = int.Parse(voter.Oib);
            Random random = new Random(seed);
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
