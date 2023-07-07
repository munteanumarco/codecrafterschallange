namespace OtpGenerator.Models;

public class VerifyOtpModel
{
    public string UserId { get; set; }
    public string Otp { get; set; }

    public VerifyOtpModel(string userId, string otp)
    {
        UserId = userId;
        Otp = otp;
    }
}