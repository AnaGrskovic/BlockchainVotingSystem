using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/votes")]
public class VoteController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IMessageQueueService _messageQueueService;

    public VoteController(
        IAuthorizationService authorizationService,
        IMessageQueueService messageQueueService)
    {
        _authorizationService = authorizationService;
        _messageQueueService = messageQueueService;
    }

    [HttpPost(Name = "CreateVote")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] string vote)
    {
        string? token = Request.Headers["Authorization"];
        if (token is null)
        {
            return Unauthorized();
        }
        bool isTokenValid = await _authorizationService.CheckTokenAsync(token);
        if (!isTokenValid)
        {
            return Unauthorized();
        }
        _messageQueueService.SendMessage(vote);
        return Ok();
    }
}
