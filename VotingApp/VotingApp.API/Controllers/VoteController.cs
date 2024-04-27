using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/votes")]
public class VoteController : ControllerBase
{
    private readonly IMessageQueueService _messageQueueService;

    public VoteController(IMessageQueueService messageQueueService)
    {
        _messageQueueService = messageQueueService;
    }

    [HttpPost(Name = "CreateVote")]
    public IActionResult Create([FromBody] string vote)
    {
        var token = Request.Headers["Authorization"];
        _messageQueueService.SendMessage(vote);
        return Ok();
    }
}
