namespace VotingApp.Contracts.Dtos;

public class BlockChainDto
{
    public IEnumerable<string> Candidates { get; set; }
    public IEnumerable<BlockDto> Blocks { get; set; } = default!;
}
