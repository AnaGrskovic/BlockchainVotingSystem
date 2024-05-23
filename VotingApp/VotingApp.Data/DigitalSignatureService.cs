using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class DigitalSignatureService : IDigitalSignatureService
{
    public Task<bool> VerifyDigitalSignature(BlockChainDto blockChainDto, string signature)
    {
        throw new NotImplementedException();
    }
}
