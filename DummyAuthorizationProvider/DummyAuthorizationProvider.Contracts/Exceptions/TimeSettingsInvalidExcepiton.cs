namespace DummyAuthorizationProvider.Contracts.Exceptions;

public class TimeSettingsInvalidException : Exception
{
    public TimeSettingsInvalidException(string message) : base(message)
    {
    }
}
