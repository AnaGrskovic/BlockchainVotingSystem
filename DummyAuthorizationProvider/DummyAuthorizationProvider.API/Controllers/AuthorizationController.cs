using DummyAuthorizationProvider.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DummyAuthorizationProvider.API.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _voterService;

    public AuthorizationController(IAuthorizationService voterService)
    {
        _voterService = voterService;
    }

    [HttpPost("get-token", Name = "GetToken")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTokenAsync([FromBody] string oib)
    {
        string? token = await _voterService.GetTokenAsync(oib);
        if (token == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(token);
        }
    }

    [HttpGet("check-token", Name = "CheckToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CheckTokenAsync()
    {
        string? token = Request.Headers["Authorization"];
        if (token == null)
        {
            return Unauthorized();
        }
        bool isValid = await _voterService.IsTokenValidAsync(token);
        if (isValid)
        {
            return Ok();
        }
        else
        {
            return Unauthorized();
        }
    }
}