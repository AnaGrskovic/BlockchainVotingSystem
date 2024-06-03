using Microsoft.Extensions.Options;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;
using VotingApp.Contracts.UoW;

namespace VotingApp.Services;

public class BlockChainService : IBlockChainService
{
    private readonly IUnitOfWork _uow;

    public BlockChainService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task CreateAsync(BlockChainDto blockChainDto)
    {
        var blockChain = new BlockChain(blockChainDto);
        _uow.BlockChains.Add(blockChain);
        await _uow.SaveChangesAsync();
    }

    public async Task<List<BlockChain>> GetAllAsync()
    {
        return await _uow.BlockChains.GetAllAsync();
    }
}
