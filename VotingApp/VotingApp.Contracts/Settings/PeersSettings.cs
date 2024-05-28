namespace VotingApp.Contracts.Settings;

public class PeersSettings
{
    public const string Section = "Peers";

    public IEnumerable<string> PublicKeys { get; set; } = default!;
}
