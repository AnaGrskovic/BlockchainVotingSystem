﻿using DummyAuthorizationProvider.Contracts.Entities;
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
        int seed = int.Parse(oib);
        Random random = new Random(seed);
        return random.Next().ToString();
    }

    public async Task CheckTokenAsync(string? token)
    {
        bool isTokenValid = await IsTokenValidAsync(token);
        if (!isTokenValid)
        {
            throw new TokenNotValidException("Token is not valid.");
        }
    }

    public async Task SetVotedAsync(string? token)
    {
        Voter voter = await GetAsync(token) ?? throw new EntityNotFoundException("There is no voter with that token.");
        voter.Voted = true;
        _uow.Voters.Update(voter);
        await _uow.SaveChangesAsync();
    }

    private async Task<List<Voter>> GetAllAsync()
    {
        return await _uow.Voters.GetAllAsync();
    }

    private async Task<bool> IsTokenValidAsync(string? token)
    {
        return await GetAsync(token) != null;
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
