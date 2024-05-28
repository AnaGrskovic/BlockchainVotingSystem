using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using VotingApp.Contracts.Dtos;
using VotingApp.Contracts.Helper;
using VotingApp.Contracts.Services;
using VotingApp.Contracts.Settings;

namespace VotingApp.Services;

public class DigitalSignatureService : IDigitalSignatureService
{
    private readonly PeersSettings _settings;

    public DigitalSignatureService(IOptions<PeersSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool VerifyDigitalSignature(BlockChainDto blockChainDto, string signature, string publicKeyPem)
    {
        var formattedPublicKeyPem = CheckAndFormatPublicKey(publicKeyPem);
        if (formattedPublicKeyPem is null)
        {
            return false;
        }

        RSAParameters publicKey = GetPublicKeyFromPem(formattedPublicKeyPem);

        var blockChainJson = JsonSerializer.Serialize(blockChainDto.Blocks);

        return VerifyMessage(publicKey, blockChainJson, signature);
    }

    private string? CheckAndFormatPublicKey(string publicKey)
    {
        var normalizedPublicKey = publicKey.Replace("\n", "").Replace("\r", "");
        foreach (string allowedPublicKey in _settings.PublicKeys)
        {
            var normalizedAllowedPublicKey = allowedPublicKey.Replace("\n", "").Replace("\r", "");
            if (normalizedPublicKey.Equals(normalizedAllowedPublicKey))
            {
                return allowedPublicKey;
            }
        }
        return null;
    }

    public static RSAParameters GetPublicKeyFromPem(string pem)
    {
        var pemReader = new StringReader(pem);
        var pemObject = new PemReader(pemReader).ReadPemObject();
        var rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(pemObject.Content, out _);
        return rsa.ExportParameters(false);
    }

    public static bool VerifyMessage(RSAParameters publicKey, string message, string encodedSignature)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportParameters(publicKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] signature = Convert.FromBase64String(encodedSignature);
            return rsa.VerifyData(messageBytes, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}

