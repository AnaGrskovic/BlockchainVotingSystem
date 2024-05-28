using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Services;
using VotingApp.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/block-chains")]
public class BlockChainController : ControllerBase
{
    private readonly IDigitalSignatureService _digitalSignatureService;
    private readonly IBlockChainService _blockChainService;

    public BlockChainController(IDigitalSignatureService digitalSignatureService, IBlockChainService blockChainService)
    {
        _digitalSignatureService = digitalSignatureService;
        _blockChainService = blockChainService;
    }

    [HttpPost(Name = "CreateBlockChain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] BlockChainDto blockChainDto)
    {
        string? signature = Request.Headers["Signature"];
        string? publicKeyPem = Request.Headers["Public-Key"];

        if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(publicKeyPem))
        {
            return Unauthorized();
        }
        var isSignatureValid = _digitalSignatureService.VerifyDigitalSignature(blockChainDto, signature, publicKeyPem);
        if (!isSignatureValid)
        {
            return Unauthorized();
        }
        await _blockChainService.CreateAsync(blockChainDto);
        return Ok();
    }
}
