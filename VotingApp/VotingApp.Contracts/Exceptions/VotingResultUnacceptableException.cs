namespace VotingApp.Contracts.Exceptions;

public class VotingResultUnacceptableException : Exception
{
    public VotingResultUnacceptableException(string message) : base(message)
    {
    }
}
