using VotingApp.Contracts.Dtos;

namespace VotingApp.Contracts.Services;

public interface IDigitalSignatureService
{
    bool VerifyDigitalSignature(PeerBlockChainDto peerBlockChainDto, string signature, string publicKeyPem);
}
