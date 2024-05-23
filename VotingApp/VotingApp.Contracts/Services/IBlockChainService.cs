using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IBlockChainService
{
    Task CreateAsync(BlockChainDto blockChainDto);
}
