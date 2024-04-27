namespace DummyAuthorizationProvider.Contracts.Exceptions;

public class TokenNotValidException : Exception
{
    public TokenNotValidException(string message) : base(message)
    {
    }
}
