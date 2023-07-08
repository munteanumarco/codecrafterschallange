using OtpGenerator.Services.Otp.Interfaces;

namespace OtpGenerator.Services.Otp.Dependencies;

public class TimeProvider : ITimeProvider
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}