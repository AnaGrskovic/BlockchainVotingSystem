using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IBlockChainResultService
{
    Task<VotingResultDto> GetVotingResultAsync();
}
