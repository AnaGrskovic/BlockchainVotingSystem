namespace VotingApp.Contracts.Exceptions;

public class VoteNotPresentException : Exception
{
    public VoteNotPresentException(string message) : base(message)
    {
    }
}
