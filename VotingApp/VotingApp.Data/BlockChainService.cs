using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;
using VotingApp.Contracts.Services;
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
}
