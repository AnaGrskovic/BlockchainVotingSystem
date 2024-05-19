using DummyAuthorizationProvider.Contracts.Enums;

namespace DummyAuthorizationProvider.Contracts.Entities;

public class Voter
{
    public Guid Id { get; set; }
    public string Oib { get; set; } = default!;
    public VoterStatus Status { get; set; }

    public Voter(Guid id, string oib)
    {
        Id = id;
        Oib = oib;
        Status = VoterStatus.Nothing;
    }
}

