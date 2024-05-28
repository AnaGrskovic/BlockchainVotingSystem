using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IDigitalSignatureService
{
    bool VerifyDigitalSignature(BlockChainDto blockChainDto, string signature, string publicKeyPem);
}
