using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface ISecureBlockChainService
{
    Task CheckAndCreateAsync(BlockChainDto blockChainDto, string? signature, string? publicKeyPem);
}
