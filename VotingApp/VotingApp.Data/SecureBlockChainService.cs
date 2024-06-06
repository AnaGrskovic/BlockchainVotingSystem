using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class SecureBlockChainService : ISecureBlockChainService
{
    private readonly ITimeService _timeService;
    private readonly IDigitalSignatureService _digitalSignatureService;
    private readonly IPeerService _peerService;
    private readonly IBlockChainService _blockChainService;

    public SecureBlockChainService(
        ITimeService timeService,
        IDigitalSignatureService digitalSignatureService,
        IPeerService peerService,
        IBlockChainService blockChainService)
    {
        _timeService = timeService;
        _digitalSignatureService = digitalSignatureService;
        _peerService = peerService;
        _blockChainService = blockChainService;
    }

    public async Task CheckAndCreateAsync(PeerBlockChainDto blockChainDto, string? signature, string? publicKeyPem)
    {
        if (!(_timeService.IsAfterVotingTime() && !_timeService.IsAfterStabilizationTime()))
        {
            throw new ForbiddenTimeException("It is not allowed to create blockchains now.");
        }

        if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(publicKeyPem))
        {
            throw new SignatureOrKeyNotPresentException("Digital signature or public key are not present in the request.");
        }
        var isSignatureValid = _digitalSignatureService.VerifyDigitalSignature(blockChainDto, signature, publicKeyPem);
        if (!isSignatureValid)
        {
            throw new SignatureNotValidException("Digital signature is not valid.");
        }

        var didPeerAlreadyCreateBlockChain = await _peerService.DoesAlreadyExistAsync(blockChainDto.Peer);
        if (didPeerAlreadyCreateBlockChain)
        {
            throw new BlockChainAlreadyCreatedException("Peer already created a blockchain.");
        }
        await _peerService.CreateAsync(blockChainDto.Peer);

        await _blockChainService.CreateAsync(blockChainDto.BlockChain);
    }
}
