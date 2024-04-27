namespace VotingApp.Contracts.Settings;

public class AuthorizationSettings
{
    public const string Section = "Authorization";

    public string BaseUrl { get; set; } = default!;
    public string CheckTokenEndpoint { get; set; } = default!;
}
