using System.Security.Cryptography;
using System.Text;
using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGenerator.Services.Otp.Dependencies;

public class SharedSecretProvider : ISharedSecretProvider
{
    public byte[] GetSharedSecret(string userId)
    {
        using var sha256 = SHA256.Create();
        var userIdBytes = Encoding.UTF8.GetBytes(userId);
        return sha256.ComputeHash(userIdBytes);
    }
}