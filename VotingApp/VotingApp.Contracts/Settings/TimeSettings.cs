namespace VotingApp.Contracts.Settings;

public class TimeSettings
{
    public const string Section = "Time";

    public DateTime BlockChainCaluldationStartTime { get; set; } = default!;
    public DateTime BlockChainCaluldationEndTime { get; set; } = default!;
}
