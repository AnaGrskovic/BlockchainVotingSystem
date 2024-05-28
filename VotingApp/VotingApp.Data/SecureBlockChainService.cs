using Azure.Core;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Exceptions;
using VotingApp.Contracts.Services;

namespace VotingApp.Services;

public class SecureBlockChainService : ISecureBlockChainService
{
    private readonly ITimeService _timeService;
    private readonly IDigitalSignatureService _digitalSignatureService;
    private readonly IBlockChainService _blockChainService;

    public SecureBlockChainService(ITimeService timeService, IDigitalSignatureService digitalSignatureService, IBlockChainService blockChainService)
    {
        _timeService = timeService;
        _digitalSignatureService = digitalSignatureService;
        _blockChainService = blockChainService;
    }

    public async Task CheckAndCreateAsync(BlockChainDto blockChainDto, string? signature, string? publicKeyPem)
    {
        if (!_timeService.IsBlockChainCalculationTime())
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
        await _blockChainService.CreateAsync(blockChainDto);
    }
}
