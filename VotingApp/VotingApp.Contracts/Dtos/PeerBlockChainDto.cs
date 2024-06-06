namespace VotingApp.Contracts.Dtos;

public class PeerBlockChainDto
{
    public PeerDto Peer { get; set; } = default!;
    public BlockChainDto BlockChain { get; set; } = default!;
}
