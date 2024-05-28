using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Entities;

namespace VotingApp.Contracts.Services;

public interface IBlockChainService
{
    Task CreateAsync(BlockChainDto blockChainDto);

    Task<List<BlockChain>> GetAllAsync();
}
