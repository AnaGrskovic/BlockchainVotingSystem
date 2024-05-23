namespace VotingApp.Contracts.Dtos;

public class BlockChainDto
{
    public IEnumerable<BlockDto> Blocks { get; set; } = default!;
}
