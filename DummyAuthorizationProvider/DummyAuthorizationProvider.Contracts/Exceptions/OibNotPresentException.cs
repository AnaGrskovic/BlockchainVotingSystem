namespace DummyAuthorizationProvider.Contracts.Exceptions;

public class OibNotPresentException : Exception
{
    public OibNotPresentException(string message) : base(message)
    {
    }
}
