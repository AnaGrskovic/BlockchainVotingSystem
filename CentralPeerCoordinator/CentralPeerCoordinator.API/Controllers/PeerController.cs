using CentralPeerCoordinator.Contracts.Dtos;
using CentralPeerCoordinator.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace CentralPeerCoordinator.API.Controllers;

[ApiController]
[Route("api/peers")]
public class PeerController : ControllerBase
{
    private readonly IPeerService _peerService;

    public PeerController(IPeerService peerService)
    {
        _peerService = peerService;
    }

    [HttpGet(Name = "GetAllPeers")]
    [ProducesResponseType(typeof(List<PeerResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _peerService.GetAllAsync());
    }

    [HttpGet("{id}", Name = "GetPeer")]
    [ProducesResponseType(typeof(PeerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        return Ok(await _peerService.GetAsync(id));
    }

    [HttpPost(Name = "CreatePeer")]
    [ProducesResponseType(typeof(PeerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] PeerRequestDto request)
    {
        return Ok(await _peerService.CreateAsync(request));
    }

    [HttpPut("{id}", Name = "UpdatePeer")]
    [ProducesResponseType(typeof(PeerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] PeerRequestDto request)
    {
        return Ok(await _peerService.UpdateAsync(id, request));
    }

    [HttpDelete("{id}", Name = "DeletePeer")]
    [ProducesResponseType(typeof(PeerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        await _peerService.DeleteAsync(id);
        return Ok();
    }
}
