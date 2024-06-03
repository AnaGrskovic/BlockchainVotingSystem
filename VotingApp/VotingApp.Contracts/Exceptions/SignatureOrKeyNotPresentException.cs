namespace VotingApp.Contracts.Exceptions;

public class SignatureOrKeyNotPresentException : Exception
{
    public SignatureOrKeyNotPresentException(string message) : base(message)
    {
    }
}
