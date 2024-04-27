using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/votes")]
public class VoteController : ControllerBase
{
    private readonly IVotingService _votingService;

    public VoteController(IVotingService votingService)
    {
        _votingService = votingService;
    }

    [HttpPost(Name = "CreateVote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] string vote)
    {
        string? token = Request.Headers["Authorization"];
        await _votingService.VoteAsync(token, vote);
        return Ok();
    }
}
