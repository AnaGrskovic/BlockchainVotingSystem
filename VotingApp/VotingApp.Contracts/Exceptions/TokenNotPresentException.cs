namespace VotingApp.Contracts.Exceptions;

public class TokenNotPresentException : Exception
{
    public TokenNotPresentException(string message) : base(message)
    {
    }
}