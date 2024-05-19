namespace DummyAuthorizationProvider.Contracts.Entities;

public class Voter
{
    public Guid Id { get; set; }
    public string Oib { get; set; } = default!;
    public bool Voted { get; set; }

    public Voter(Guid id, string oib)
    {
        Id = id;
        Oib = oib;
        Voted = false;
    }
}

