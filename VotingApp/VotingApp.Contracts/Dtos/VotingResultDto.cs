namespace VotingApp.Contracts.Dtos;

public class VotingResultDto
{
    public int? NumberOfVotes { get; set; } = default!;
    public Dictionary<string, int>? NumberOfVotesPerCandidate { get; set; } = default!;

    public VotingResultDto(int numberOfVotes, Dictionary<string, int> numberOfVotesPerCandidate)
    {
        NumberOfVotes = numberOfVotes;
        NumberOfVotesPerCandidate = numberOfVotesPerCandidate;
    }

    public VotingResultDto(int numberOfVotes)
    {
        NumberOfVotes = numberOfVotes;
    }

    public VotingResultDto()
    {
    }
}
