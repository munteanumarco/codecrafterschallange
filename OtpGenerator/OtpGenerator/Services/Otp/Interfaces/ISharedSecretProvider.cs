namespace OtpGenerator.Services.Otp.Interfaces;

public interface ISharedSecretProvider
{
    byte[] GetSharedSecret(string userId);
}