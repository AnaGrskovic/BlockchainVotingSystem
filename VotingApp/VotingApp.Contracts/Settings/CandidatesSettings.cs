namespace VotingApp.Contracts.Settings;

public class CandidatesSettings
{
    public const string Section = "Candidates";

    public IEnumerable<string> Candidates { get; set; } = default!;
}

