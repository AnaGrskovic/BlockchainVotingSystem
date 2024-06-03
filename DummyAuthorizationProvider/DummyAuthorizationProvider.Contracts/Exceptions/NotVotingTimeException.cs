namespace DummyAuthorizationProvider.Contracts.Exceptions;

public class NotVotingTimeException : Exception
{
    public NotVotingTimeException(string message) : base(message)
    {
    }
}
