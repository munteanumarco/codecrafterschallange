namespace OtpGenerator.Services.Otp.Interfaces;

public interface IOtpGenerator
{
    string GenerateOtp(byte[] sharedSecret, long timeStep);
}