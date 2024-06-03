namespace VotingApp.Contracts.Exceptions;

public class SignatureNotValidException : Exception
{
    public SignatureNotValidException(string message) : base(message)
    {
    }
}
