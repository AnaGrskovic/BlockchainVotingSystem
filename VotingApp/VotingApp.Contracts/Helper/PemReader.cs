using System.Text;

namespace VotingApp.Contracts.Helper;

public class PemReader
{
    private readonly StringReader _reader;

    public PemReader(StringReader reader)
    {
        _reader = reader;
    }

    public PemObject ReadPemObject()
    {
        var content = new StringBuilder();
        string line;
        while ((line = _reader.ReadLine()) is not null)
        {
            if (line.StartsWith("-----BEGIN "))
            {
                continue;
            }

            if (line.StartsWith("-----END "))
            {
                break;
            }

            content.Append(line);
        }

        return new PemObject(Convert.FromBase64String(content.ToString()));
    }
}