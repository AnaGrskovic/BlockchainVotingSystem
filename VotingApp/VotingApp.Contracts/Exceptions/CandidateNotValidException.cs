namespace VotingApp.Contracts.Exceptions;

public class CandidateNotValidException : Exception
{
    public CandidateNotValidException(string message) : base(message)
    {
    }
}
