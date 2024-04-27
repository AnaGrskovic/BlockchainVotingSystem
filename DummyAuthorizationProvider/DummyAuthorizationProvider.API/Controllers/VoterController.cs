using DummyAuthorizationProvider.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DummyAuthorizationProvider.API.Controllers;

[ApiController]
[Route("api/voters")]
public class VoterController : ControllerBase
{
    private readonly IVoterService _voterService;

    public VoterController(IVoterService voterService)
    {
        _voterService = voterService;
    }

    [HttpPost("get-token", Name = "GetToken")]
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

    [HttpPost("check-token", Name = "CheckToken")]
    public async Task<IActionResult> CheckTokenAsync([FromBody] string token)
    {
        bool isValid = await _voterService.IsTokenValidAsync(token);
        if (isValid)
        {
            return Ok();
        }
        else
        {
            return Forbid();
        }
    }
}