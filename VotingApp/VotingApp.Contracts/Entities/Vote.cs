namespace VotingApp.Contracts.Entities;

public class Vote
{
    public Guid Id { get; set; } = default!;
    public string Candidate { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = default!;

    public Vote(string candidate)
    {
        Candidate = candidate;
        CreatedAt = DateTime.UtcNow;
    }
}
