namespace VotingApp.Contracts.Exceptions;

public class UnsuccessfulSerializationException : Exception
{
    public UnsuccessfulSerializationException(string message) : base(message)
    {
    }
}
