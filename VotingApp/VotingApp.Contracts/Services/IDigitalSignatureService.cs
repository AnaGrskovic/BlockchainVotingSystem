using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IDigitalSignatureService
{
    Task<bool> VerifyDigitalSignature(BlockChainDto blockChainDto, string signature);
}
