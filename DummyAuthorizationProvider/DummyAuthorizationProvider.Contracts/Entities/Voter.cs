namespace DummyAuthorizationProvider.Contracts.Entities;

public class Voter
{
    public Guid Id { get; set; }
    public string Oib { get; set; } = default!;
}

