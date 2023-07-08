namespace OtpGenerator.Services.Otp.Interfaces;

public interface IOtpService
{
    string GetOtp(string userId);
    bool VerifyOtp(string userId, string otp);
}