namespace VotingApp.Contracts.Exceptions;

public class AuthProviderException : Exception
{
    public AuthProviderException(string message) : base(message)
    {
    }
}