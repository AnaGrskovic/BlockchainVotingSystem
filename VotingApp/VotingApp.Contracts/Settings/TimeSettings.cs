﻿namespace VotingApp.Contracts.Settings;

public class TimeSettings
{
    public const string Section = "Time";

    public DateTime BlockChainCalculationStartTime { get; set; } = default!;
    public DateTime BlockChainCalculationEndTime { get; set; } = default!;
    public DateTime BlockChainStabilizationEndTime { get; set; } = default!;
}
