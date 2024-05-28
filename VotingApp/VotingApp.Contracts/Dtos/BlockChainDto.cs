namespace VotingApp.Contracts.Dtos;

public class BlockChainDto
{
    public IEnumerable<string> Candidates { get; set; } = default!;
    public IEnumerable<BlockDto> Blocks { get; set; } = default!;
}
