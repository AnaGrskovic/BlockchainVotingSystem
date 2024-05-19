using DummyAuthorizationProvider.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DummyAuthorizationProvider.API.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;

    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("get-token", Name = "GetToken")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTokenAsync([FromBody] string? oib)
    {
        return Ok(await _authorizationService.GetTokenAsync(oib));
    }

    [HttpGet("check-token-not-voted", Name = "CheckTokenNotVoted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CheckTokenNotVotedAsync()
    {
        string? token = Request.Headers["Authorization"];
        await _authorizationService.CheckTokenNotVotedAsync(token);
        return Ok();
    }

    [HttpGet("check-token-voted", Name = "CheckTokenVoted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CheckTokenVotedAsync()
    {
        string? token = Request.Headers["Authorization"];
        await _authorizationService.CheckTokenVotedAsync(token);
        return Ok();
    }

    [HttpPut("set-voted", Name = "SetVoted")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetVotedAsync()
    {
        string? token = Request.Headers["Authorization"];
        await _authorizationService.SetVotedAsync(token);
        return Ok();
    }
}