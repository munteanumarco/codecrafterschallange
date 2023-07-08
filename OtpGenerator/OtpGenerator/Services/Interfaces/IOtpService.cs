namespace OtpGenerator.Services.Interfaces;

public interface IOtpService
{
    string GetOtp(string userId);
    bool VerifyOtp(string userId, string otp);
}