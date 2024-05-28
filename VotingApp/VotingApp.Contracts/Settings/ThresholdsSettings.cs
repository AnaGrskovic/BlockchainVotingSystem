namespace VotingApp.Contracts.Settings;

public class ThresholdsSettings
{
    public const string Section = "Thresholds";

    public double MinimalPercentageOfCorrectBlockChains { get; set; } = default!;
}
