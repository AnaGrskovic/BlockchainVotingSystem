namespace VotingApp.Contracts.Exceptions;

public class InvalidSettingsException : Exception
{
    public InvalidSettingsException(string message) : base(message)
    {
    }
}

