using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/results")]
public class BlockChainResultController : ControllerBase
{
    private readonly IBlockChainResultService _blockChainResultService;

    public BlockChainResultController(IBlockChainResultService blockChainResultService)
    {
        _blockChainResultService = blockChainResultService;
    }

    [HttpGet(Name = "GetVotingResults")]
    [ProducesResponseType(typeof(VotingResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetVotingResultAsync()
    {
        return Ok(await _blockChainResultService.GetVotingResultAsync());
    }
}