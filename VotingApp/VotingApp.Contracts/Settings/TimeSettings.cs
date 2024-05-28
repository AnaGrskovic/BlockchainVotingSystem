namespace VotingApp.Contracts.Settings;

public class TimeSettings
{
    public const string Section = "Time";

    public DateTime VotingEndTime { get; set; } = default!;
    public DateTime CalculationEndTime { get; set; } = default!;
}
