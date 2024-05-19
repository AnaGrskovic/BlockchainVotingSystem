namespace DummyAuthorizationProvider.Contracts.Exceptions;

public class VoterAlreadyVotedException : Exception
{
    public VoterAlreadyVotedException(string message) : base(message)
    {
    }
}