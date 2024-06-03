namespace VotingApp.Contracts.Settings;

public class AuthorizationSettings
{
    public const string Section = "Authorization";

    public string BaseUrl { get; set; } = default!;
    public string CheckTokenNotVotedEndpoint { get; set; } = default!;
    public string SetVotedEndpoint { get; set; } = default!;
}
