namespace VotingApp.Contracts.Dtos;

public class BlockDto
{
    public int Nonce { get; set; } = default!;
    public long TimeStamp { get; set; } = default!;
    public string Data { get; set; } = default!;
    public string PreviousHash { get; set; } = default!;
    public string Hash { get; set; } = default!;
}
