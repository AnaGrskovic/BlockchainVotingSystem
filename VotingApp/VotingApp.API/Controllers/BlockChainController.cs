using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/block-chains")]
public class BlockChainController : ControllerBase
{
    private readonly ISecureBlockChainService _secureBlockChainService;

    public BlockChainController(ISecureBlockChainService secureBlockChainService)
    {
        _secureBlockChainService = secureBlockChainService;
    }

    [HttpPost(Name = "CreateBlockChain")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateAsync([FromBody] BlockChainDto blockChainDto)
    {
        string? signature = Request.Headers["Signature"];
        string? publicKeyPem = Request.Headers["Public-Key"];

        await _secureBlockChainService.CheckAndCreateAsync(blockChainDto, signature, publicKeyPem);
        return Ok();
    }
}
