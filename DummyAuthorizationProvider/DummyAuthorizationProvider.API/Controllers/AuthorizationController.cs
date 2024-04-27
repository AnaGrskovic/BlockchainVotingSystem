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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTokenAsync([FromBody] string? oib)
    {
        return Ok(await _authorizationService.GetTokenAsync(oib));
    }

    [HttpGet("check-token", Name = "CheckToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CheckTokenAsync()
    {
        string? token = Request.Headers["Authorization"];
        await _authorizationService.CheckToken(token);
        return Ok();
    }
}