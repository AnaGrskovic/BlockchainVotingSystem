using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface ISecureBlockChainService
{
    Task CheckAndCreateAsync(PeerBlockChainDto blockChainDto, string? signature, string? publicKeyPem);
}
