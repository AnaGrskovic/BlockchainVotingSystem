﻿using DummyAuthorizationProvider.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DummyAuthorizationProvider.API.Controllers;

[ApiController]
[Route("api/authorization")]
public class AuthorizationController : ControllerBase
{
    private readonly IVoterService _voterService;

    public AuthorizationController(IVoterService voterService)
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

    [HttpGet("check-token", Name = "CheckToken")]
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