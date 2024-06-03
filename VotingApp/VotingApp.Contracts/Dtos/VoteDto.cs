namespace VotingApp.Contracts.Dtos;

public class VoteDto
{
    public string Token { get; set; } = default!;
    public string Vote { get; set; } = default!;

    public VoteDto(string token, string vote)
    {
        Token = token;
        Vote = vote;
    }
}

