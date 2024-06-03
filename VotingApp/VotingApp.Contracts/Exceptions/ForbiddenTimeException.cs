namespace VotingApp.Contracts.Exceptions;

public class ForbiddenTimeException : Exception
{
    public ForbiddenTimeException(string message) : base(message)
    {
    }
}
