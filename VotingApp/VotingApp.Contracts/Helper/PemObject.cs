namespace VotingApp.Contracts.Helper;

public class PemObject
{
    public byte[] Content { get; }

    public PemObject(byte[] content)
    {
        Content = content;
    }
}