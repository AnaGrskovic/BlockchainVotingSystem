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

    [HttpPost(Name = "GetToken")]
    public IActionResult GetToken([FromBody] string oib)
    {
        return Ok(_voterService.GetToken(oib));
    }

    [HttpPost(Name = "CheckToken")]
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