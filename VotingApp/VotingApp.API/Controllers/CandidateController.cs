using Microsoft.AspNetCore.Mvc;
using VotingApp.Contracts.Services;

namespace VotingApp.API.Controllers;

[ApiController]
[Route("api/candidates")]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet(Name = "GetAllCandidates")]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok(_candidateService.GetAll());
    }
}